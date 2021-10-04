using UnityEngine;

namespace UTK.Animation
{
    public static class AnimatorParameterIDs
    {
        public static readonly int Idle = Animator.StringToHash("Idle");
        
        // Attack
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int AttackStack = Animator.StringToHash("Attackstack");
        public static readonly int AttackIdle = Animator.StringToHash("Attackidle");
        
        public static readonly int Skill1 = Animator.StringToHash("Skillv1");
        public static readonly int Skill2 = Animator.StringToHash("Skillv2");
        public static readonly int Skill3 = Animator.StringToHash("Skillv3");
        public static readonly int Skill4 = Animator.StringToHash("Skillv4");
        
        // Jump
        public static readonly int Grounded = Animator.StringToHash("Grounded");
        public static readonly int Jump = Animator.StringToHash("Jump");
        public static readonly int FreeFall = Animator.StringToHash("FreeFall");
        
        // Move
        public static readonly int Speed = Animator.StringToHash("Speed");
    }
}
