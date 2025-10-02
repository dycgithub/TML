

using UnityEngine;

public class BasePanel
{
   public UIType uiType;
   public GameObject activePanel;
   
   public BasePanel(UIType uiType)
   {
       this.uiType = uiType;
   }

   public virtual void OnStart()
   {
       UIMethods.GetInstance().GetOrAddComponent<CanvasGroup>(activePanel).interactable = true;
   }

   public virtual void OnEnable()
   {
       UIMethods.GetInstance().GetOrAddComponent<CanvasGroup>(activePanel).interactable = true;
   }

   public virtual void OnDisable()
   {
       UIMethods.GetInstance().GetOrAddComponent<CanvasGroup>(activePanel).interactable = false;
   }

   public virtual void OnDestroy()
   {
       UIMethods.GetInstance().GetOrAddComponent<CanvasGroup>(activePanel).interactable = false;
   }
}
