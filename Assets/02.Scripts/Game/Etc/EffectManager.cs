using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager
{
    private Dictionary<string, IEffectable> effectDict = new Dictionary<string, IEffectable>();

    public void Initialize()
    {
        // effectDict 값 추가
        Add<GoldEffect>(new GoldEffect(Managers.Wave.FieldGolds));
    }

    /// <summary>
    /// IEffectable형식으로된 클래스 추가 자세한건 Initialize 추가
    /// </summary>
    /// <typeparam name="T">클래스 접근 키</typeparam>
    /// <param name="effectable">값</param>
    public void Add<T>(T effectable) where T : IEffectable
    {
        effectDict.Add(typeof(T).Name, effectable);
    }

    /// <summary>
    /// 사용하고싶은 곳에 언제 어디서나 접근가능
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void InvokeEffect<T>() where T : IEffectable
    {
        if (effectDict.TryGetValue(typeof(T).Name, out var result))
        {
            result.StartEffect();
        }
    }

    /// <summary>
    /// 유니테스크 파괴
    /// </summary>
    private void Destory()
    {
        foreach (var pair in effectDict)
        {
            if (effectDict.TryGetValue(pair.Key, out var result))
            {
                if(result is IUsableUniTask)
                {
                    (result as IUsableUniTask).TokenDestroy();
                }
            }
        }
    }
}
