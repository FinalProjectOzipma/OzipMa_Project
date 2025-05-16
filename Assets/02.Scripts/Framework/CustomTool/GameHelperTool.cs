using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHelperTool : EditorWindow
{
    [MenuItem("Tools/Game Helper Tool")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GameHelperTool));
    }

    private void OnGUI()
    {
#if UNITY_EDITOR
        GUILayout.Label("Wave Editor", EditorStyles.boldLabel);

        GUILayout.Label($"현재 게임 상태 : {Enum.GetName(typeof(Enums.WaveState),Managers.Wave.CurrentState)}");

        GUILayout.Label($"현재 스테이지 : {Managers.Player.CurrentStage}");
        GUILayout.Label($"현재 웨이브 : {Managers.Player.CurrentWave}");

        if (GUILayout.Button("MYUNIT ALL KILL", GUILayout.Width(256)))
        {
            if (Managers.Instance == null) return;

            EntityAllKill<MyUnitController>(Managers.Wave.CurMyUnitList);
        }

        if (GUILayout.Button("ENEMY ALL KILL", GUILayout.Width(256)))
        {
            if (Managers.Instance == null) return;

            EntityAllKill<EnemyController>(Managers.Wave.CurEnemyList);
        }

      
        if (GUILayout.Button("웨이브 건너뛰기", GUILayout.Width(256)))
        {
            if (Managers.Instance == null) return;
            if (Managers.Wave.CurrentState != Enums.WaveState.Playing)
            {
                Util.Log("현재 게임 상태가 Playing중일때만 가능합니다.");
                return;
            }
            EntityAllKill<MyUnitController>(Managers.Wave.CurMyUnitList);
            EditorApplication.isPaused = true;
            Managers.Wave.MainCore.ApplyDamage(100000000f);

            PlayerManager playerManager = Managers.Player;
            var stages = Util.TableConverter<DefaultTable.Stage>(Managers.Data.Datas[Enums.Sheet.Stage]);
            int EndNumber = stages[playerManager.CurrentKey].StageEnd;

            if (++playerManager.CurrentWave % 10 == 0)
            {
                if (++playerManager.CurrentStage > EndNumber)
                    playerManager.CurrentKey = Mathf.Min(++playerManager.CurrentKey, stages.Count - 1); // 스테이지 끝이면 현재 키를 늘린다.
                playerManager.CurrentWave = 0;
            }

            Managers.Player.OnStageWave();
        }

        if(Managers.Instance != null) Managers.Game.IsGodMode = EditorGUILayout.Toggle("갓 모드 활성화", Managers.Game.IsGodMode);
    }
#endif

    private void EntityAllKill<T>(List<T> list) where T : EntityController
    {
        List<T> entities = list;
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].GetComponent<IDamagable>().ApplyDamage(1000000f);
        }
    }

}
