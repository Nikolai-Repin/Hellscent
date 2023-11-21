using System;

public class MiscEvents
{
    public event Action onTreasureCollected;

    public void TreasureCollected() {
        if (onTreasureCollected != null) {
            onTreasureCollected();
        }
    }
}