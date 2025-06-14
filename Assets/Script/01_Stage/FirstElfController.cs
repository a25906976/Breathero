using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

// 綁在 treeElf Prefab 上
public class FirstElfController : MonoBehaviour
{
    public ElfStates state; 
    [SerializeField] FirstElfData_SO elfData;

    // 視野範圍
    [SerializeField] float sightRadius;
    // 走路範圍
    [SerializeField] float walkingRange;
    // 可能的弱點
    [SerializeField] List<GameObject> aimICON;
    //樹精身上長的火焰
    public GameObject firePrefab;
    public Transform parentElf;
    private List<GameObject> firesOnElf;

    public int health = 4;
    private NavMeshAgent agent;
    private Animator anim;

    // 即玩家
    private Transform attackTarget;
    private Vector3 wayPoint;
    private Vector3 originPos;
    private float lastDistance;
    private CapsuleCollider ElfCollider;
    // 被選中的四個弱點
    private Dictionary<int,GameObject> selectedAimICON;
    // 避免被打特效還未結束，又被觸發
    private bool attacked;
    public bool isBoss;
    public bool BossChangeMode;
    private bool hasAttack = false; //樹精攻擊過了

    private AudioManager audioManager;

    private void OnEnable() {

        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        health = 4;
        BossChangeMode = false;
        ElfCollider = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();    
        anim = GetComponent<Animator>();

        agent.isStopped = false;

        EventManager.ElfMoveEvent += elfMove;
        // 弱點
        selectedAimICON = new Dictionary<int,GameObject>();
        // 選四個
        for(int i=0;i<4;i++)
        {
            int tmp = Random.Range(0,4-i);
            selectedAimICON.Add(aimICON[tmp].GetComponent<HitAimICON>().aimIndex,aimICON[tmp]);
            aimICON[tmp].SetActive(true);
            aimICON.RemoveAt(tmp);
        }

        // 精靈的初始位置
        originPos = transform.position;

        // 第一隻生成的是站樁怪
        if(elfData.index == 0)
        {
            state = ElfStates.IDLE;
        }
        // 非站樁怪
        else
        {
            state = ElfStates.WALK;
            anim.SetBool("Walk",true);
        }
        GetNewWayPoint();
    }
    private void OnDisable() {
        //if (aimICON.Count >= 0)
        //{
        //    aimICON.RemoveAt(0);
        //}
        //aimICON.RemoveAt(0);
        EventManager.ElfMoveEvent -= elfMove;
    }

    // 呼吸教學時，精靈移動到攻擊範圍內
    void elfMove(Vector3 target)
    {
        anim.SetBool("Walk",true);
        agent.destination = new Vector3(target.x, transform.position.y, transform.position.z);
        state = ElfStates.TUTORIAL;
    }

    void Update()
    {
        if(state == ElfStates.WALK){
            // 玩家在視野範圍內
            if(FoundPlayer())
            {
                agent.destination = attackTarget.position;
                //Debug.Log(Vector3.Distance(agent.destination, transform.position));
                // 精靈和玩家接近到一定距離，則開始攻擊玩家
                if(Vector3.Distance(agent.destination, transform.position) <= agent.stoppingDistance)
                {
                    anim.SetBool("Attack",true);
                    state = ElfStates.ATTACK;
                    Debug.Log("attttaaaack");
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    {
                        if (!hasAttack)
                        {
                            hasAttack = true;
                            audioManager.TreeAttact();
                        }
                    }
                    else if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    {
                        hasAttack= false;
                    }
                    // 攻擊玩家！！！
                }
            }
            // 散步
            else{
                if(Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                {
                    GetNewWayPoint();
                    //audioManager.Walk();
                    // 走路音效（但我不確定這邊是不是走路的地方）
                }
                else
                {
                    // 用來避免撞牆 (撞牆即無法靠近wayPoint，所以更新wayPoint)
                    if(lastDistance <= Vector3.Distance(wayPoint, transform.position))
                        GetNewWayPoint();
                    agent.destination = wayPoint;
                    lastDistance = Vector3.Distance(wayPoint, transform.position);
                }
            }
        }
        else if(state == ElfStates.TUTORIAL)
        {
            // 走到攻擊範圍內就停止
            // 不知道為何stopping distance不是設定的1
            if(Vector3.Distance(agent.destination,transform.position) <= 0.1f)
            {
                anim.SetBool("Idle",true);
                state = ElfStates.IDLE;
            }
        }
        else if(state == ElfStates.ATTACK)
        {
            // 更新玩家位置

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                if (!hasAttack)
                {
                    hasAttack = true;
                    audioManager.TreeAttact();
                }
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                hasAttack = false;
            }
            agent.destination = attackTarget.position;
            if(Vector3.Distance(agent.destination, transform.position) > agent.stoppingDistance)
            {
                anim.SetBool("Walk",true);
                state = ElfStates.WALK;
            }
        }

    }

    bool FoundPlayer()
    {   
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach(var target in colliders)
        {
            if(target.CompareTag("Player"))
            {   
                attackTarget = target.transform;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }

    // 取得下個目標點 (此時玩家不在視野範圍內，精靈會在，以其生成位置為圓心的走路範圍內散步)
    void GetNewWayPoint()
    {
        float randomX = Random.Range(-walkingRange,walkingRange);
        float randomZ = Random.Range(-walkingRange,walkingRange);
        Vector3 randomPoint = new Vector3(originPos.x + randomX, transform.position.y, originPos.z + randomZ);
        NavMeshHit hit;
        // 這個點要可以走 (NavMesh)
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, walkingRange, NavMesh.AllAreas) ? hit.position : transform.position;
    }

    //// 被玩家偵測到 (出現弱點)
    //private void OnTriggerStay(Collider other) {
    //    // Detection即Front Object
    //    if(other.tag=="Detection")
    //    {
    //        if(selectedAimICON!=null && state!=ElfStates.DEAD)
    //            foreach(var icon in selectedAimICON)
    //            {
    //                if(!icon.Value.activeSelf)
    //                    icon.Value.SetActive(true);
    //            }
    //    }
    //}

    //// 未被玩家偵測到 (弱點消失)
    //private void OnTriggerExit(Collider other) {
    //    if(other.tag=="Detection")
    //    {
    //        if(selectedAimICON!=null)
    //            foreach(var icon in selectedAimICON)
    //            {
    //                icon.Value.SetActive(false);
    //            }
    //    }
    //}

    public void Dead(int index)
    {
     
        // selectedAimICON.Remove(index);
        state = ElfStates.DEAD;
        if(!isBoss)
        {
            ElfCollider.isTrigger = true;
            anim.SetBool("Dead",true);
            foreach (var icon in selectedAimICON)
            {
                icon.Value.SetActive(false);
            }
            StartCoroutine(FadeOut(transform.Find("Box027").gameObject));
            Destroy(gameObject, 2f);
        }
        else
        {
            foreach (var icon in selectedAimICON)
            {
                // 刪了所有著火特效
                foreach (Transform child in icon.Value.transform) {
                    GameObject.Destroy(child.gameObject);
                }
                icon.Value.SetActive(false);
            }
            if(!attacked)
                StartCoroutine(BeAttackedEffect(transform.Find("Box027").gameObject));
            anim.SetBool("Walk",false);    
            anim.SetBool("BeAttacked",false);
            anim.speed = 1;
            BossChangeMode = true;
        }
            
    }

    public void BeAttacked(int index)
    {
        Transform firePos = selectedAimICON[index].transform;

        GameObject.Instantiate(firePrefab, firePos);
        

        // selectedAimICON.Remove(index);
        if(!attacked)
            StartCoroutine(BeAttackedEffect(transform.Find("Box027").gameObject));
    }

    private IEnumerator FadeOut(GameObject ObjFade)
    {
        Renderer objRenderer = ObjFade.transform.GetComponent<Renderer>();
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
        
        // 切回被擊中前的狀態
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
        else if(stateTmp==ElfStates.DEAD)
        {
            state = stateTmp;
        }
        // (打到一個以上的弱點的話?)
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
