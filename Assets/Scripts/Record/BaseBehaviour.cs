using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseBehaviour:ISerializationCallbackReceiver
{
    public BehaviourType type;
    public BaseBehaviour(BehaviourType type)
    {
        this.type = type;
    }

    public virtual void OnAfterDeserialize() { }
    public virtual void OnBeforeSerialize() { }

}

public enum BehaviourType
{
    Move,
    Jump,
    Climb,
    Interactive //交互
}

public class MoveBehaviour : BaseBehaviour
{
    public Vector3 dir;
    public Vector3 delta;

    public MoveBehaviour(Vector3 dir, Vector3 delta):base(BehaviourType.Move)
    {
        this.dir = dir;
        this.delta = delta;
    }
}

public class JumpBehaviour : BaseBehaviour
{
    public JumpBehaviour() : base(BehaviourType.Jump) { }
}

public class ClimbBehaviour: BaseBehaviour
{
    public ClimbBehaviour() : base(BehaviourType.Climb) { }

}

public class InteractiveBehaviour : BaseBehaviour
{
    public InteractiveBehaviour() : base(BehaviourType.Interactive) { }
}


