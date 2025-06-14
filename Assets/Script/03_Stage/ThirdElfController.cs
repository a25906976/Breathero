using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 綁在 fireElf Object
// 綁在 fireElf Object �?
// 綁在 fireElf Object 上
public class ThirdElfController : MonoBehaviour
{
    public ElfStates state; 
    [SerializeField] FirstElfData_SO elfData;

    // 視野範圍
    [SerializeField] float sightRadius;
    // 走路範圍
    [SerializeField] float walkingRange;
    [SerializeField] List<GameObject> aimICON;

    // 集氣特效
    [SerializeField] ParticleSystem sparks;
    [SerializeField] ParticleSystem fireBall;
    // 噴火特效
    [SerializeField] ParticleSystem fireThrower;

    public int health = 2;
    private NavMeshAgent agent;
    private Animator anim;

    private GameObject attackTarget;
    private Vector3 wayPoint;
    private Vector3 originPos;
    private float lastDistance;
    private CapsuleCollider ElfCollider;
    private Dictionary<int,GameObject> selectedAimICON;
    private bool charging;
    public static float Pure_Charge_Time;
    public static bool Stop_Charge = false;

    // 避免被打特效還未結束，又被觸發
    private bool attacked;
    private IEnumerator coroutine;

    public bool isBoss;
    public bool BossChangeMode;
    public float stopFire;
    // public AudioManager03 audioManager;

    // disable 的 script 似乎仍會觸發 Awake()

    private void OnEnable() {
        charging = false;
        health = 2;
        BossChangeMode = false;
        ElfCollider = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();    
        anim = GetComponent<Animator>();

        agent.isStopped = false;

        EventManager.ElfMoveEvent += elfMove;
        // // 弱點
        // selectedAimICON = new Dictionary<int,GameObject>();
        // // 選四個
        // for(int i=0;i<4;i++)
        // {
        //     int tmp = Random.Range(0,6-i);
        //     selectedAimICON.Add(aimICON[tmp].GetComponent<HitAimICON3>().aimIndex,aimICON[tmp]);
        //     aimICON.RemoveAt(tmp);
        // }

        originPos = transform.position;
        // 教學用的第一二隻是站樁怪
        if(elfData.index == 0 )
        {
            state = ElfStates.IDLE;
        }
        // 非站樁怪
        else
        {
            Debug.Log("Walk");
            state = ElfStates.WALK;
            anim.SetBool("Walk",true);
        }
        GetNewWayPoint();

        if(isBoss)
        {
            sparks.gameObject.SetActive(true);
            fireBall.gameObject.SetActive(true);
            fireThrower.gameObject.SetActive(true);
        }

        // 一開始不會噴火
        if(sparks.isPlaying)
            sparks.Stop();
        if(fireBall.isPlaying)
            fireBall.Stop();
        if(fireThrower.isPlaying)
            fireThrower.Stop();
    }
    private void OnDisable() {
        EventManager.ElfMoveEvent -= elfMove;
        if(isBoss)
        {
            aimICON.RemoveAt(0);
            aimICON.RemoveAt(0);
            sparks.gameObject.SetActive(false);
            fireBall.gameObject.SetActive(false);
            fireThrower.gameObject.SetActive(false);
        }
    }

    // 呼吸教學時，精靈移動到攻擊範圍內
    void elfMove(Vector3 target)
    {
        anim.SetBool("Walk",true);
        // agent.destination = new Vector3(target.x, transform.position.y, transform.position.z);
        // state = ElfStates.TUTORIAL;
    }

    void Update()
    {
        if(state == ElfStates.WALK){

            if (isBoss) agent.speed = 0.2f;
            else agent.speed = 0.8f;
            if (FoundPlayer())
            {
                agent.destination = attackTarget.transform.position;

                if(isBoss)
                {
                    if(!charging && Vector3.Distance(transform.position, attackTarget.transform.position)<=6)
                    {
                        // charging = true;
                        // anim.SetBool("CHARGE",true);
                        state = ElfStates.CHARGE;
                    }
                }
                else
                {
                    // 正在集氣就不會進入if
                    // 距離Player 5公尺就開始集氣
                    if(!charging && Vector3.Distance(transform.position, attackTarget.transform.position)<=5)
                    {
                        // charging = true;
                        // anim.SetBool("CHARGE",true);
                        state = ElfStates.CHARGE;
                    }
                }

            }
            else{
                if(Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                {
                    GetNewWayPoint();
                }
                else
                {
                    // 用來避免撞牆
                    if(lastDistance <= Vector3.Distance(wayPoint, transform.position))
                        GetNewWayPoint();
                    agent.destination = wayPoint;
                    lastDistance = Vector3.Distance(wayPoint, transform.position);
                }
            }
        }
        //else if (state == ElfStates.IDLE)
        //{
        //    anim.SetBool("Idle", true);
        //    //agent.isStopped = true;
        //}
        // 走到攻擊範圍內就停止
        else if(state == ElfStates.TUTORIAL)
        {
            // 不知道為何stopping distance不是設定的1
            if(Vector3.Distance(agent.destination,transform.position) <= 0.1f)
            {
                anim.SetBool("Idle",true);
                state = ElfStates.IDLE;
                // 不動了
                agent.isStopped = true;
                Debug.Log(agent.isStopped);
            }
        }
        else if(state == ElfStates.ATTACK)
        {
            // elf停止走動，噴火
            agent.isStopped = true;
            transform.LookAt(new Vector3(attackTarget.transform.position.x,transform.position.y,attackTarget.transform.position.z));
            // audioManager.FireAttact();
        }
        else if(state == ElfStates.CHARGE)
        {
            if(isBoss)
            {
                if(!charging){
                    anim.SetBool("Fire",true);
                    agent.isStopped = true;
                    coroutine = Fire();
                    StartCoroutine(coroutine);
                }
                transform.LookAt(new Vector3(attackTarget.transform.position.x,transform.position.y,attackTarget.transform.position.z));
            }
            else{
                if(!charging)
                {
                    coroutine = Fire();
                    StartCoroutine(coroutine);
                }
                // Fire()觸發一次即可，切回走路模式(會追蹤Player)
                state = ElfStates.WALK;
            }
            charging = true;
        }
        else if(state == ElfStates.COOLDOWN)
        {
            charging = false;
            // Elf停下休息
        }
        // else if(state == ElfStates.StandUp)
        // {
        //     AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);
        //     if(animInfo.IsName("StandUp") && animInfo.normalizedTime>=1f)
        //     {
        //         // 魔王起身
        //         state = ElfStates.WALK;
        //         anim.SetBool("Walk",true);
        //         agent.isStopped = false;
        //     }
        // }
        else if (state == ElfStates.PURE_CHARGE)
        {
           
            StartCoroutine(Pure_Charge());
            // 為了不重複觸發Pure_Charge()
            state = ElfStates.IDLE;

            // 感覺沒用上
            if (Stop_Charge == true)
            {
                StopCoroutine(Pure_Charge());
            }
            Stop_Charge = false;
        }
        else if (state == ElfStates.PURE_FIRE)
        {
            StartCoroutine(Pure_Fire());
            // 為了不重複觸發Pure_Fire()
            state = ElfStates.IDLE;
        }

    }

    bool FoundPlayer()
    {   
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach(var target in colliders)
        {
            if(target.CompareTag("Player"))
            {   
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }


    //集氣而已(教學用)
    IEnumerator Pure_Charge()
    {
        float elpasedTime = 0f;
        fireBall.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        if (!sparks.isPlaying)
            sparks.Play();
        if (!fireBall.isPlaying)
            fireBall.Play();
        // 集氣秒數由Pure_Charge_Time決定
        while (elpasedTime <= Pure_Charge_Time)
        {
            // 火球漸漸變大
            elpasedTime += Time.deltaTime;
            if(isBoss)
                fireBall.transform.localScale = Vector3.Lerp(new Vector3(0.2f, 0.2f, 0.2f), new Vector3(0.3f, 0.3f, 0.3f), elpasedTime / Pure_Charge_Time);
            else
                fireBall.transform.localScale = Vector3.Lerp(new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.3f, 0.3f, 0.3f), elpasedTime / Pure_Charge_Time);
            yield return null;
        }
        state = ElfStates.IDLE;
    }


    //純噴火而已(教學用
    IEnumerator Pure_Fire()
    {
        // 集氣結束
        if (sparks.isPlaying)
            sparks.Stop();
        if (fireBall.isPlaying)
            fireBall.Stop();
        // 噴火
        if (!fireThrower.isPlaying)
            fireThrower.Play();
        
        anim.SetBool("Attack", true);
        // 噴火秒數
        yield return new WaitForSeconds(3);
        if (fireThrower.isPlaying)
            fireThrower.Stop();

        anim.SetBool("CoolDown", true);
        state = ElfStates.IDLE;
        
    }

    public IEnumerator Fire()
    {
        // 不知道為什麼，一定要先用fireBall.isStopped或!fireBall.isPlaying來檢查
        // 火球的Play()才會有效，否則只有閃電

        float elpasedTime = 0f;
        // 集氣初始只出現火花閃電和小火球
        fireBall.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        if(!sparks.isPlaying)
            sparks.Play();
        if(!fireBall.isPlaying)
            fireBall.Play();

        // 集氣8秒 (隨便設的)
        while(elpasedTime<=18f)
        {
            agent.isStopped = true;
            // 火球漸漸變大
            elpasedTime += Time.deltaTime;
            if(isBoss)
                fireBall.transform.localScale = Vector3.Lerp(new Vector3(0.2f,0.2f,0.2f),new Vector3(0.3f,0.3f,0.3f), elpasedTime/6f);
            else
                fireBall.transform.localScale = Vector3.Lerp(new Vector3(0.1f,0.1f,0.1f),new Vector3(0.3f,0.3f,0.3f), elpasedTime/6f);
            //Debug.Log(elpasedTime);
            yield return null;
        }

        if(stopFire>0f)
        {
            yield return new WaitForSeconds(stopFire);
            if(sparks.isPaused)
                sparks.Play();
            if(fireBall.isPaused)
                fireBall.Play();
        }

        // 集氣結束
        if(sparks.isPlaying)
        {
            sparks.Stop();
        }
            
        if(fireBall.isPlaying)
        {
            fireBall.Stop();
        }
            

        // 開始噴火
        if(!fireThrower.isPlaying)
            fireThrower.Play();
        // 切成攻擊模式
        state = ElfStates.ATTACK;
        anim.SetBool("Attack",true);
        // 吐4秒火焰 (隨便設的)
        yield return new WaitForSeconds(4);
        if(fireThrower.isPlaying)
            fireThrower.Stop();
        // 進入冷卻時間
        anim.SetBool("CoolDown",true);
        //agent.speed = 0.8f;
        state = ElfStates.COOLDOWN;
        // 冷卻20秒 (隨便設的)
        yield return new WaitForSeconds(20);
        // 繼續走動
        agent.speed = 0.8f;
        state = ElfStates.WALK;
        anim.SetBool("Walk",true);
        agent.isStopped = false;
        // if(isBoss)
        // {
        //     anim.SetBool("StandUp",true);
        //     state = ElfStates.StandUp;
        // }
        // else
        // {
        //     state = ElfStates.WALK;
        //     anim.SetBool("Walk",true);
        //     agent.isStopped = false;
        // }

    }

    void GetNewWayPoint()
    {
        float randomX = Random.Range(-walkingRange,walkingRange);
        float randomZ = Random.Range(-walkingRange,walkingRange);
        Vector3 randomPoint = new Vector3(originPos.x + randomX, transform.position.y, originPos.z + randomZ);
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, walkingRange, 1) ? hit.position : transform.position;
    }

    // // 被玩家偵測到 (出現弱點)
    // private void OnTriggerStay(Collider other) {
    //     if(other.tag=="Detection")
    //     {
    //         if(selectedAimICON!=null)
    //             foreach(var icon in selectedAimICON)
    //             {
    //                 if(!icon.Value.activeSelf)
    //                     icon.Value.SetActive(true);
    //             }
    //     }
    // }

    // // 未被玩家偵測到 (弱點消失)
    // private void OnTriggerExit(Collider other) {
    //     if(other.tag=="Detection")
    //     {
    //         if(selectedAimICON!=null)
    //             foreach(var icon in selectedAimICON)
    //             {
    //                 icon.Value.SetActive(false);
    //             }
    //     }
    // }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "FireBall")
        {
            if(!charging)
            {
                
                health--;
                Debug.Log("health"+health);
                if(health==0)
                {
                    state = ElfStates.DEAD;
                    Debug.Log("Dead");
                    Dead(0);
                }
                else if(health>0)
                {
                    BeAttacked(0);
                }
            }

        }  
    }

    public void Dead(int index)
    {
        // selectedAimICON.Remove(index);
        state = ElfStates.DEAD;
        if(!isBoss)
        {
            ElfCollider.isTrigger = true;
            anim.SetBool("Dead",true);
            StartCoroutine(FadeOut(transform.Find("Box027").gameObject));
            Destroy(gameObject, 2f);
        }
        else
        {            
            if(sparks.isPlaying)
            {
                sparks.Clear();
                sparks.Stop();
            }
            if(fireBall.isPlaying)
            {
                fireBall.Clear();
                fireBall.Stop();
            }   
            if(fireThrower.isPlaying)
            {
                fireThrower.Clear();
                fireThrower.Stop();
            }
            BossChangeMode = true;
            if(!attacked)
            {
                StartCoroutine(BeAttackedEffect(transform.Find("Box027").gameObject));
                if(coroutine!=null)
                    StopCoroutine(coroutine);
            }
            anim.SetBool("Walk",false);    
            anim.SetBool("BeAttacked",false);
            anim.speed = 1;
        }
            
    }

    public void BeAttacked(int index)
    {
        if(!attacked)
            StartCoroutine(BeAttackedEffect(transform.Find("Box027").gameObject));
    }

    private IEnumerator FadeOut(GameObject ObjFade)
    {
        Renderer objRenderer = ObjFade.transform.GetComponent<Renderer>();
        // switch to Fade Mode
        // 對Boss而言，只取到第一個Material
        objRenderer.material.SetFloat("_Mode", 2);
        Material objMaterial = objRenderer.material;
        objMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        objMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        objMaterial.SetInt("_ZWrite", 0);
        objMaterial.DisableKeyword("_ALPHATEST_ON");
        objMaterial.EnableKeyword("_ALPHABLEND_ON");
        objMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        objMaterial.renderQueue = 3000;
        Color cInitialColor = objRenderer.material.color;
        Color cTargetColor = new Color(255f,0f,0f,0f);
        float elpasedTime = 0f;
        float elpasedDuration = 2f;
        while(elpasedTime<elpasedDuration)
        {
            elpasedTime += Time.deltaTime;
            objRenderer.material.color = Color.Lerp(cInitialColor,cTargetColor,elpasedTime/elpasedDuration);
            yield return null;
        }
    }

    private IEnumerator BeAttackedEffect(GameObject obj)
    {
        attacked = true;
        ElfStates stateTmp = state;
        if(state!=ElfStates.COOLDOWN)
            anim.SetBool("BeAttacked",true);
        state = ElfStates.ATTACKED;
        agent.destination = transform.position;
        Renderer objRenderer = obj.transform.GetComponent<Renderer>();
        // switch to Fade Mode
        // objRenderer.material.SetFloat("_Mode", 2);
        objRenderer.material.SetFloat("_Surface", 1f);
        Material objMaterial = objRenderer.material;
        objMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        objMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        objMaterial.SetInt("_ZWrite", 0);
        objMaterial.DisableKeyword("_ALPHATEST_ON");
        objMaterial.EnableKeyword("_ALPHABLEND_ON");
        objMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        objMaterial.renderQueue = 3000;
        Color elfColor = objRenderer.material.color;
        Color cInitialColor = objRenderer.material.color;
        Color cTargetColor = new Color(255f,0f,0f,200f);
        float elpasedTime = 0f;
        float elpasedDuration1 = 0.1f;
        while(elpasedTime<elpasedDuration1)
        {
            elpasedTime += Time.deltaTime;
            objRenderer.material.color = Color.Lerp(cInitialColor,cTargetColor,elpasedTime/elpasedDuration1);
            yield return null;
        }
        cInitialColor = cTargetColor;
        cTargetColor = elfColor;
        elpasedTime = 0f;
        while(elpasedTime<elpasedDuration1)
        {
            elpasedTime += Time.deltaTime;
            objRenderer.material.color = Color.Lerp(cInitialColor,cTargetColor,elpasedTime/elpasedDuration1);
            yield return null;
        }
        // switch to opaque mode
        // objRenderer.material.SetFloat("_Mode", 0);
        objRenderer.material.SetFloat("_Surface", 0f);
        objMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        objMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        objMaterial.SetInt("_ZWrite", 1);
        objMaterial.DisableKeyword("_ALPHATEST_ON");
        objMaterial.DisableKeyword("_ALPHABLEND_ON");
        objMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        objMaterial.renderQueue = -1;
        
        // Debug.Log("stateTmp"+stateTmp);
        // Debug.Log("state"+state);
        if(stateTmp==ElfStates.IDLE)
        {
            anim.SetBool("Idle",true);
            state = stateTmp;
        }
        else if(stateTmp==ElfStates.WALK)
        {
            state = stateTmp;
            anim.SetBool("Walk",true);
        }
        else if(stateTmp==ElfStates.ATTACK)
        {
            state = stateTmp;
            anim.SetBool("Attack",true);
        }
        else if(stateTmp==ElfStates.COOLDOWN)
        {
            state = stateTmp;
        }
        else if(stateTmp==ElfStates.CHARGE)
        {
            state = stateTmp;
        }
        else if(stateTmp==ElfStates.DEAD)
        {
            state = stateTmp;
        }
        // 不知道是否有這種情況
        else {
            state = ElfStates.WALK;
            anim.SetBool("Walk",true);
        }
        attacked = false;
        yield return null;

    }

    // 僅在Editor模式顯示
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);   
    }

}
