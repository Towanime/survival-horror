using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LycanStates {

    Inactive,
    WaitingForRespawn,
    CalculatingSpawnPosition,
    WaitingForFirstContact,
    StaringContestWithPlayer,
    GameOverSequenceStarted,
    GameOverSequenceEnded
}
