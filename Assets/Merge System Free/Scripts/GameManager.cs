using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public Slot[] slots;

    private Vector3 _target;
    private ItemInfo carryingItem;

    public float energy = 25f;
    public float money;
    private float margeCost = 185f;
    private float margeSame = 1370f;

    public GameObject[] goals;
    public bool isMergeOver;

    private Dictionary<int, Slot> slotDictionary;

    int[] numbers = { 0, 3, 6, 9 };

    [Header("Button")]
    [SerializeField] private Button apply;
    [SerializeField] private Button add;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("DOTWeen effect")]
    [SerializeField] private float duraiton;
    [SerializeField] private float strenght;
    [SerializeField] private int vibrato;
    [SerializeField] private float randomness;

    [SerializeField] private Animator animator;

    [SerializeField] private Slider energySlider;

    [Header("Particle")]
    [SerializeField] private ParticleSystem moneyEffect;

    [SerializeField] private ParticleSystem[] sliderParticle;

    [SerializeField] private ParticleSystem[] carParticle;

    private bool okey = false;

    private float currentVelocity = 0f;


    [Header("margeOverAnimation")]
    [HideInInspector] public GameObject[] lastItems;
    [SerializeField]public Transform lastTrasnform;
    [SerializeField]public ParticleSystem[] margeOverParticle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Utils.InitResources();

        if (PlayerPrefs.HasKey(nameof(money)) == false)
        {
            PlayerPrefs.SetFloat(nameof(money) , 5000);
        } 
        if (PlayerPrefs.HasKey(nameof(energy)) == false)
        {
            PlayerPrefs.SetFloat(nameof(energy), 5);
        }

        moneyEffect.Stop();
    }

    private void Start()
    {
        energy = PlayerPrefs.GetFloat(nameof(energy));
        money = PlayerPrefs.GetFloat(nameof(money));

        slotDictionary = new Dictionary<int, Slot>();

        levelText.text = SceneController.sceneNumber.ToString();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }

        okey = RandomItemOnStart();
    }

    //handle user input
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SendRayCast();
        }
            
        if (Input.GetMouseButton(0) && carryingItem )  
        {
            OnItemSelected();
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Drop item
            SendRayCast();
        }

        float currentScore = Mathf.SmoothDamp(energySlider.value, energy, ref currentVelocity, 175 * Time.deltaTime);
        energySlider.value = currentScore;
    }
    void SendRayCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        //we hit something
        if (hit.collider != null)
        {
            //we are grabbing the item in a full slot
            var slot = hit.transform.GetComponent<Slot>();
            if (slot.state == SlotState.Full && carryingItem == null)
            {
                AudioManager.instance.PlaySFX("margeSource");
                var itemGO = (GameObject)Instantiate(Resources.Load("Prefabs/ItemDummy"));
                itemGO.transform.position = slot.transform.position;
                itemGO.transform.localScale = Vector3.one * 2;

                carryingItem = itemGO.GetComponent<ItemInfo>();
                carryingItem.InitDummy(slot.id, slot.currentItem.id);

                slot.ItemGrabbed();
            }
            //we are dropping an item to empty slot
            else if (slot.state == SlotState.Empty && carryingItem != null)
            {
                slot.CreateItem(carryingItem.itemId);
                Destroy(carryingItem.gameObject);
            }

            //we are dropping to full
            else if (slot.state == SlotState.Full && carryingItem != null)
            {
                //check item in the slot
                if (slot.currentItem.id == carryingItem.itemId)
                {
                    print("merged");
                    OnItemMergedWithTarget(slot.id);

                }
                else
                {
                    OnItemCarryFail();
                }
            }

        }
        else
        {
            if (!carryingItem)
            {   
                return;
            }
            OnItemCarryFail();
        }
    }

    void OnItemSelected()
    {
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _target.z = 0;
        var delta = 10 * Time.deltaTime;

        delta *= Vector3.Distance(transform.position, _target);
        carryingItem.transform.position = Vector3.MoveTowards(carryingItem.transform.position, _target, delta);
    }

    void OnItemMergedWithTarget(int targetSlotId)
    {
        var slot = GetSlotById(targetSlotId);

        switch (carryingItem.itemId)
        {
            case 1:
                GetSelectedItem(0, targetSlotId);
                break;
            case 4:
                GetSelectedItem(1, targetSlotId);
                break;
            case 7:
                GetSelectedItem(2, targetSlotId);
                break;
            case 10:
                GetSelectedItem(3, targetSlotId);
                break;
            default:
                Destroy(slot.currentItem.gameObject);
                slot.CreateItem(carryingItem.itemId + 1);
                break;
        }
        Destroy(carryingItem.gameObject);

    }
    void OnItemCarryFail()  
    {
        var slot = GetSlotById(carryingItem.slotId);
        slot.CreateItem(carryingItem.itemId);
        Destroy(carryingItem.gameObject);
    }

    public void PlaceRandomItem()
    {

        if (money >= margeCost)
        {
            int randomIndex = Random.Range(0, 4);
            int randomIndexSlat = numbers[randomIndex];

            if (AllSlotsOccupied())
            {
                Debug.Log("No empty slot available!");
                return;
            }

            var rand = UnityEngine.Random.Range(0, slots.Length);
            var slot = GetSlotById(rand);

            while (slot.state == SlotState.Full)
            {
                rand = UnityEngine.Random.Range(0, slots.Length);
                slot = GetSlotById(rand);
            }

            slot.CreateItem(randomIndexSlat);

            if (SlotState.Full != null && okey)
            {
                money -= margeCost;
                AudioManager.instance.PlaySFX("addSource");
                add.transform.DOShakeScale(duraiton, strenght, vibrato, randomness);
            }

            scoreText.text = money.ToString();
        }
    }


    bool AllSlotsOccupied()
    {
        foreach (var slot in slots)
        {
            if (slot.state == SlotState.Empty)
            {
                //empty slot found
                return false;
            }
        }
        //no slot empty 
        return true;
    }

    Slot GetSlotById(int id)
    {
        return slotDictionary[id];
    }

    void GetSelectedItem(int a, int targetSlotId)
    {
        var slot = GetSlotById(targetSlotId);
        if (goals[a].active == false)
        {
            Vector3 pos = goals[a].transform.position;
            goals[a].transform.position = slot.transform.position;
            goals[a].active = true;
            goals[a].transform.DOMove(pos, 1f).OnComplete(() => SettingsSliderParticle(a));
            Destroy(slot.currentItem.gameObject);
            slot.state = SlotState.Empty;
            AudioManager.instance.PlaySFX("winSource");
            energy += 25f;
            PlayerPrefs.SetFloat(nameof(energy), energy);
        }
        else
        {
            Destroy(slot.currentItem.gameObject);
            slot.state = SlotState.Empty;
            money += margeSame;
            scoreText.text = money.ToString();
            moneyEffect.transform.position = slot.transform.position + new Vector3(0,0,-2);
            moneyEffect.Play();
        }

        isMergeOver = isAllItemActive();

        if (isMergeOver)
        {
            AudioManager.instance.PlaySFX("carDonwSource");
            apply.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            apply.transform.DOShakeScale(duraiton,strenght,vibrato,randomness);
            animator.SetTrigger("donwCar");
            
            StartCoroutine(wait1scn(0.75f));

            Margever();
                
        }

        IEnumerator wait1scn(float time)
        {
            yield return new WaitForSeconds(time);

            for (int i = 0; i < 2; i++)
            {
                carParticle[i].Play();
            }
        }
    }

    void SettingsSliderParticle(int a)
    {
        sliderParticle[a].Play();
    }

    bool isAllItemActive()
    {
        foreach (var item in goals)
        {
            if (item.active == false)
            {
                return false;
            }
        }
        return true;
    }

    bool RandomItemOnStart()
    {
        for (int i = 0; i < 9; i++)
        {
            PlaceRandomItem();
        }
        return true;
    }

    private void Margever()
    {
        lastItems = GameObject.FindGameObjectsWithTag("Item");

        for (int i = 0; i < lastItems.Length; i++)
        {
            //lastItems[i].transform.position = lastTrasnform.position;
            lastItems[i].transform.DOMove(lastTrasnform.position, 2f);
            StartCoroutine(MargeOverTime(2f, lastItems));
            //Destroy(lastItems[i]);
        }
    }

    IEnumerator MargeOverTime(float t , GameObject[] �tem)
    {
        yield return new WaitForSeconds(t);
        for (int i = 0; i < �tem.Length; i++)
        {
            Destroy(�tem[i]);
            money += 25f;
            scoreText.text = money.ToString();
            PlayerPrefs.SetFloat(nameof(money), money);
        }
        for (int a = 0; a < margeOverParticle.Length; a++)
        {
            margeOverParticle[a].Play();
        }
    }
}
