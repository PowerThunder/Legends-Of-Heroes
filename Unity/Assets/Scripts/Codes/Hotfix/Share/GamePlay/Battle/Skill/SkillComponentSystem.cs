﻿using System;
using System.Collections.Generic;

namespace ET
{
    [FriendOf(typeof(SkillComponent))]
    [FriendOf(typeof(BattleUnitComponent))]
    public static class SkillComponentSystem
    {
        [ObjectSystem]
        public class SkillComponentAwakeSystem : AwakeSystem<SkillComponent>
        {
            protected override void Awake(SkillComponent self)
            {
                self.Awake();
            }
        }

	

        private static void Awake(this SkillComponent self)
        {
            
        }
        
        /// <summary>
        /// 释放技能
        /// </summary>
        /// <param name="self"></param>
        /// <param name="absType"></param>
        /// <param name="index"></param>
        public static void SpellSkill(this SkillComponent self, ESkillAbstractType absType, int index = 0)
        {
            //
            Skill skill = null;
            self.GetParent<BattleUnitComponent>()?.TryGetSkill(absType, index, out skill);
            if (skill == null || skill.IsInCd())
                return;
            skill.StartSpell();
        }
    }

   
}