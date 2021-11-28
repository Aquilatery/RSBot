﻿using RSBot.Core;
using RSBot.Core.Event;
using RSBot.Core.Objects;

namespace RSBot.Protection.Components.Player
{
    internal class SkillHealthHandler
    {
        public static void Initialize()
        {
            SubscribeEvents();
        }

        /// <summary>
        /// Subscribes the events.
        /// </summary>
        private static void SubscribeEvents()
        {
            EventManager.SubscribeEvent("OnUpdateHP", OnUpdateHP);
        }

        /// <summary>
        /// Cores the on player HPMP update.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private static void OnUpdateHP()
        {
            if (!Kernel.Bot.Running)
                return;

            if ((Game.Player.BadEffect & BadEffect.Zombie) == BadEffect.Zombie)
                return;

            if (PlayerConfig.Get<bool>("RSBot.Protection.checkUseSkillHP")) 
                return;

            var skillId = PlayerConfig.Get<uint>("RSBot.Protection.HpSkill");
            if (skillId == 0)
                return;

            var minHealth = PlayerConfig.Get<int>("RSBot.Protection.numPlayerSkillHPMin", 50);

            var healthPercent = ((double)Game.Player.Health / (double)Game.Player.MaximumHealth) * 100;
            if (healthPercent > minHealth)
                return;

            var skill = Game.Player.Skills.GetSkillInfoById(skillId);
            if (skill == null)
                return;

            Log.Debug($"Using HP skill: {skill}");
            Game.Player.CastBuff(skill.Id);
        }
    }
}