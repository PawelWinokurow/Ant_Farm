using System;
using UnityEngine;
using FighterNamespace;
public class DigJob
{
    public string id { get; set; }
    public Vector3 destination { get; set; }
    public FloorHexagon hex { get; set; }
    public Action Cancel { get; set; }
    public Scorpion scorpion { get; set; }

    public DigJob(FloorHexagon hex, Vector3 destination, Scorpion scorpion) : this(hex, destination)
    {
        this.scorpion = scorpion;
    }
    public DigJob(FloorHexagon hex, Vector3 destination)
    {
        id = hex.id;
        this.hex = hex;
        this.destination = destination;
        Cancel = () =>
        {
            scorpion.SetState(new PatrolState(scorpion));
            scorpion.path = null;
        };
    }
}