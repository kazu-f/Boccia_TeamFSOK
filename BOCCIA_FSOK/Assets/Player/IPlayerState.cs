using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public abstract class IPlayerState
    {
        protected PlayerController m_player;

        abstract public void Init(PlayerController controller);
        abstract public void Enter();
        abstract public void Leave();
        abstract public void Execute();

        //—LŒø‚É‚È‚Á‚Ä‚¢‚é‚©H
        abstract public bool IsEnable();
        //ˆ—‚ğ~‚ß‚Ä‚¢‚é‚©H
        abstract public bool IsStop();

    }
}