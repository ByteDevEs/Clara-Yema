using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuAutoSeleccion : MonoBehaviour
{
    public List<Selectable> botones;
    [SerializeField] int index = 0;

    public Button botonVolver;
    
    private void Update()
    {
        
        if(botones[index].gameObject.activeSelf)
            botones[index].Select();
        else
            Mover(1);
        
        if(botones.Count == 0)
            return;

        if (botones[index] is TMP_Dropdown)
        {
            TMP_Dropdown dropdown = botones[index] as TMP_Dropdown;
            if(dropdown.IsExpanded)
            {
                dropdown.RefreshShownValue();
                //Get the scrollbar
                Scrollbar scrollBar = dropdown.GetComponentInChildren<Scrollbar>();
                //Set the vertical scroll position to the selected option
                scrollBar.value = 1f - (float)dropdown.value / (dropdown.options.Count - 1);
                return;
            }
        }
        botones[index].Select();
    }

    public void Select()
    {
        print("Select");
        //If is a button
        if (botones[index] is Button)
        {
            Button boton = botones[index] as Button;
            boton.onClick.Invoke();
        }
        //If is a toggle
        else if (botones[index] is Toggle)
        {
            Toggle toggle = botones[index] as Toggle;
            toggle.isOn = !toggle.isOn;
        }
        else if (botones[index] is TMP_Dropdown)
        {
            TMP_Dropdown dropdown = botones[index] as TMP_Dropdown;

            // Simulate a click on the dropdown by opening it after a short delay
            if (dropdown.IsExpanded)
            {
                // If it's open, simulate a click to close it
                dropdown.Hide();
            }
            else
            {
                // If it's closed, simulate a click to open it
                StartCoroutine(OpenDropdownAfterDelay(dropdown));
            }
        }
    }

    private IEnumerator OpenDropdownAfterDelay(TMP_Dropdown dropdown)
    {
        // Wait for the end of the frame
        yield return null;

        // Open the dropdown
        dropdown.Show();
    }
    
    public void Volver()
    {
        if (botonVolver != null)
        {
            botonVolver.onClick.Invoke();
        }
    }
    
    // Update is called once per frame
    public void Mover(int mod)
    {
        if(botones.Count == 0)
            return;

        if (botones[index] is TMP_Dropdown)
        {
            TMP_Dropdown dropdown = botones[index] as TMP_Dropdown;
            if(dropdown.IsExpanded)
            {
                dropdown.value += mod;
                if (dropdown.value < 0)
                {
                    dropdown.value = dropdown.options.Count - 1;
                }
                else if (dropdown.value >= dropdown.options.Count)
                {
                    dropdown.value = 0;
                }
                return;
            }
        }
        
        //Deseleccionar el boton actual
        //botones[index].OnDeselect(null);
        index += mod;
        if (index < 0)
        {
            index = botones.Count - 1;
        }
        else if (index >= botones.Count)
        {
            index = 0;
        }
        
        if(botones[index].gameObject.activeSelf)
            botones[index].Select();
        else
            Mover(mod);
    }
}
