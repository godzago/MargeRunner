using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour 
{
    public static GameController instance;

    public Slot[] slots;

    private Vector3 _target;
    private ItemInfo carryingItem;

    private int slotnumber;

    private Dictionary<int, Slot> slotDictionary;

    public GameObject[] goals;

    //int[] numbers = new int[11, 12, 13, 14];
    //int randomIndex = Random.Range(0, 3);

    int[] numbers = { 0, 3, 6 };




    private void Awake() {
        instance = this;
        Utils.InitResources();
    }

    private void Start() 
    {
        slotDictionary = new Dictionary<int, Slot>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }
    }

    //handle user input
    private void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            SendRayCast();
        }

        if (Input.GetMouseButton(0) && carryingItem)
        {
            OnItemSelected();
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Drop item
            SendRayCast();
        }

    }

    void SendRayCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
 
        //we hit something
        if(hit.collider != null)
        {
            //we are grabbing the item in a full slot
            var slot = hit.transform.GetComponent<Slot>();
            if (slot.state == SlotState.Full && carryingItem == null)
            {
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

    //void OnItemMergedWithTarget(int targetSlotId)
    //{
    //    var slot = GetSlotById(targetSlotId);
    //    Destroy(slot.currentItem.gameObject);

    //    slot.CreateItem(carryingItem.itemId + 1);

    //    if (carryingItem.itemId == 3)
    //    {
    //       ///
    //    }

    //    Destroy(carryingItem.gameObject);
    //}

    void OnItemMergedWithTarget(int targetSlotId)
    {
        var slot = GetSlotById(targetSlotId);

        switch (carryingItem.itemId)
        {
            case 1:
                goals[0].active = true;
                Destroy(slot.currentItem.gameObject);
                slot.state = SlotState.Empty;
                break;
            case 4:
                goals[1].active = true;
                Destroy(slot.currentItem.gameObject);
                slot.state = SlotState.Empty;
                break;
            case 7:
                goals[2].active = true;
                Destroy(slot.currentItem.gameObject);
                slot.state = SlotState.Empty;
                
                break;
            case 10:
                goals[3].active = true;
                Destroy(slot.currentItem.gameObject);
                slot.state = SlotState.Empty;
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

        slot.CreateItem(0);
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

    

}