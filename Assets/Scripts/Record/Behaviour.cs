using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DataType
{
    Vector,
    Float,
    Bool
}

[System.Serializable]
public class Behaviour 
{
    public float deltaHorizontalDisplacement;//水平位移增量
    public float deltaVerticalDisplacement;//竖直位移增量

    public float deltaYAxis; //Y轴旋转增量
    //public bool ;

    //动画的参数
    public float forward;
    public float turn;
    public bool onGround;
    public bool climb;
    public float jump;
    public float jumpLeg;
    public bool climbToTop;
    public bool carry;
    public bool applyRootMotion;

    //
    public float animatorSpeed;
    public float deltaTime;
    




    public Behaviour()
    {
        deltaHorizontalDisplacement = 0;
        deltaVerticalDisplacement = 0;
        deltaYAxis = 0;
        //type = BehaviourType.Idle;
        forward = 0;
        turn=0;
        onGround =false;
        climb=false;
        jump=0;
        jumpLeg = 0;
        animatorSpeed = 1;
        deltaTime = 0.02f;
        climbToTop = false;
        carry = false;
        applyRootMotion = true;
    }

}

