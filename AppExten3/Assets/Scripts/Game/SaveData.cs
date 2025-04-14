using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class PlayerSaveData {
    public int health;
    public float[] position;
}

[System.Serializable]
public class InventoryItem {
    public int id;
    public int quantity;
}

[System.Serializable]
public class InventorySaveData {
    public List<InventoryItem> items = new List<InventoryItem>();
}

[System.Serializable]
public class SaveData {
    public PlayerSaveData player;
    public InventorySaveData inventory;
    public int sceneIndex;
    public int progression;
}

