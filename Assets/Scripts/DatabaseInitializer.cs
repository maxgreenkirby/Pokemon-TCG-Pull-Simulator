using UnityEngine;
using System.Collections.Generic;

public class DatabaseInitalizer : MonoBehaviour
{
    [SerializeField] private Database _database;
    [SerializeField] private List<Card> _cards; 

    private void Awake()
    {
        _database.Initialize(_cards);
    }
}
