using UnityEngine;

namespace Scripts.UI
{
    public class NoticePresenter: BasePresenter<NoticeView, NoticeModel>
    {
        public void Setup(string notice, string amount)
        {
            Model.NoticeText = notice;
            Model.NoticeAmountText = amount;

            View.UpdateUI(Model);
        }
    }
}
