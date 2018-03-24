using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace EMSP.UI.Dialogs.WiringEditor
{
    public class TabNavigation : MonoBehaviour
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        public bool findFirstSelectable = false;

        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods

        void Update()
        {
            if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    Back();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Tab)) 
            {
                Forward();
            }
        }

        void Forward() // Down, Right, Up, Left
        {
            if (EventSystem.current != null)
            {
                GameObject selected = EventSystem.current.currentSelectedGameObject;

                //try and find the first selectable if there isn't one currently selected
                //only do it if the findFirstSelectable is true
                //you may not always want this feature and thus
                //it is disabled by default
                if (selected == null && findFirstSelectable)
                {
                    Selectable found = (Selectable.allSelectables.Count > 0) ? Selectable.allSelectables[0] : null;

                    if (found != null)
                    {
                        //simple reference so that selected isn't null and will proceed
                        //past the next if statement
                        selected = found.gameObject;
                    }
                }

                if (selected != null)
                {
                    Selectable current = (Selectable)selected.GetComponent("Selectable");

                    if (current != null)
                    {
                        Selectable nextDown = current.FindSelectableOnDown();
                        Selectable nextUp = current.FindSelectableOnUp();
                        Selectable nextRight = current.FindSelectableOnRight();
                        Selectable nextLeft = current.FindSelectableOnLeft();

                        if (nextDown != null)
                        {
                            nextDown.Select();
                        }
                        else if (nextRight != null)
                        {
                            nextRight.Select();
                        }
                        else if (nextUp != null)
                        {
                            nextUp.Select();
                        }
                        else if (nextLeft != null)
                        {
                            nextLeft.Select();
                        }
                    }
                }
            }
        }

        void Back() // Up, Left, Down, Right
        {
            if (EventSystem.current != null)
            {
                GameObject selected = EventSystem.current.currentSelectedGameObject;

                //try and find the first selectable if there isn't one currently selected
                //only do it if the findFirstSelectable is true
                //you may not always want this feature and thus
                //it is disabled by default
                if (selected == null && findFirstSelectable)
                {
                    Selectable found = (Selectable.allSelectables.Count > 0) ? Selectable.allSelectables[0] : null;

                    if (found != null)
                    {
                        //simple reference so that selected isn't null and will proceed
                        //past the next if statement
                        selected = found.gameObject;
                    }
                }

                if (selected != null)
                {
                    Selectable current = (Selectable)selected.GetComponent("Selectable");

                    if (current != null)
                    {
                        Selectable nextDown = current.FindSelectableOnDown();
                        Selectable nextUp = current.FindSelectableOnUp();
                        Selectable nextRight = current.FindSelectableOnRight();
                        Selectable nextLeft = current.FindSelectableOnLeft();

                        if (nextUp != null)
                        {
                            nextUp.Select();
                        }
                        else if (nextLeft != null)
                        {
                            nextLeft.Select();
                        }
                        else if(nextDown != null)
                        {
                            nextDown.Select();
                        }
                        else if (nextRight != null)
                        {
                            nextRight.Select();
                        }
                    }
                }
            }
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}



