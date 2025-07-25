using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;

    public List<CardDataSO> cardDataList;   // 游戏中所有可能出现的卡牌

    [Header("卡牌库")]
    public CardLibrarySO newGameCardLibrary; // 新游戏初始化的卡牌库
    public CardLibrarySO currentCardLibrary; // 当前游戏使用的卡牌库




    private void Awake()
    {
        InitializeCardDataList();

        foreach (var item in newGameCardLibrary.cardLibraryList)
        {
            currentCardLibrary.cardLibraryList.Add(item);
        }

    }

    private void InitializeCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCardDataLoaded;


    }

    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("No CardData Found!");
        }
    }

    public GameObject GetCardObject()
    {
        return poolTool.GetObjectFromPool();
    }

    public void DiscardCard(GameObject cardobj)
    {
        poolTool.ReturnObjectToPool(cardobj);
    }
}
