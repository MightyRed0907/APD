using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HWAttemptType
{
    Successful,
    Unsuccessful,
    Canceled
}

[SerializeField]
public class HandwashSession
{
    public HWAttemptType attemptType;
    public float totalSessionTime;
    
}

public class HandwashLog : ScriptableObject
{
    public float totalTimeInSink;
    public int totalHandwashAttempts;
    public int totalSuccessfulAttempts;
    public int totalUnsuccessfulAttempts;
    public int totalCanceledAttempts;
}
