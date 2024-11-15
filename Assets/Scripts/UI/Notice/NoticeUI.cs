using Scripts.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NoticeUI : MonoBehaviour
{
    public static Action<string, string> Show;

    public NoticePresenter noticePrefab;


    private void OnEnable()
    {
        Show += OnShow;
    }

    private void OnDisable()
    {
        Show -= OnShow;
    }

    public void OnShow(string notice, string amount = "")
    {
        var ui = Instantiate(noticePrefab, transform).GetComponent<NoticePresenter>();
        ui.gameObject.SetActive(true);
        ui.Setup(notice, amount);
    }
}
