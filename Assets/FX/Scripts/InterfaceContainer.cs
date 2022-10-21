using UnityEngine;
using UnityEngine.AI;

public interface IAnt
    {
    //  float health { get; set; } //A variable
    Transform _transform { get; set; }
    NavMeshAgent agent { get; set; }
}
