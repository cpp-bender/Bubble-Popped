using System.Collections.Generic;

public class ReferenceContanier : SingletonMonoBehaviour<ReferenceContanier>
{
    public MexicanWaveController mexicanWaveController;
    public List<DoorController> doors;
    public int ActiveDoorIndex { get; set; }

    private DoorController activeDoor;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        SetFirstActiveDoor();
    }

    private void SetFirstActiveDoor()
    {
        activeDoor = doors[ActiveDoorIndex];
    }

    public DoorController GetActiveDoor()
    {
        return activeDoor;
    }

    public MexicanWaveController GetMexicanWaveObject()
    {
        return mexicanWaveController;
    }

    public void SetActiveDoor(int activeDoorIndex)
    {
        if (activeDoorIndex < doors.Count)
        {
            activeDoor = doors[activeDoorIndex];
        }
    }
}
