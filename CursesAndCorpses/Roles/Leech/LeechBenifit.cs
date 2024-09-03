using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiraAPI.Utilities.Assets;
using UnityEngine;

public class LeechBenefit
{
    public Action<LeechRole> Action { get; private init; }
    public Func<LeechRole, bool> isAllowedToRun { get; private init; }
    public LoadableResourceAsset Sprite { get; private set; }

    public LeechBenefit(Action<LeechRole> action, Func<LeechRole, bool> condition, LoadableResourceAsset sprite)
    {
        this.Action = action;
        this.isAllowedToRun = condition;
        this.Sprite = sprite;
    }

    public static LeechBenefit CompleteTasksEffect = new LeechBenefit(r =>
    {
        var incompleteTasks = r.Player.myTasks.ToArray().Where(t => !t.IsComplete).ToList();
        int tasksAutoCompleted = 0;
        foreach (var task in incompleteTasks)
        {
            if (tasksAutoCompleted > incompleteTasks.Count / 2)
            {
                break;
            }

            if (!task.IsComplete)
            {
                tasksAutoCompleted++;
                r.Player.RpcCompleteTask(task.Id);
            }
        }
        return;
    }, r =>
    {
        return r.Player.myTasks.ToArray().Where(t => !t.IsComplete).Count() > 4;
    }, null);
    public static LeechBenefit IncreaseVision = new LeechBenefit(r => r.hasIncreasedVision = true, r => !r.hasIncreasedVision, CursesAndCorpsesAssets.LeechLightSprite);
    public static LeechBenefit DecreaseUploadSpeed = new LeechBenefit(r => r.uploadBuff = true, r => !r.uploadBuff, CursesAndCorpsesAssets.LeechUploadSprite);
    public static LeechBenefit SabotageQuickFix = new LeechBenefit(r => r.sabotageQuickFix = true, r => !r.sabotageQuickFix, CursesAndCorpsesAssets.LeechQuickFixSprite);

    public static List<LeechBenefit> AllBenifits = new List<LeechBenefit>()
    {
        CompleteTasksEffect,
        IncreaseVision,
        DecreaseUploadSpeed,
        SabotageQuickFix
    };
}

