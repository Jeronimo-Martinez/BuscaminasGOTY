using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GetValueDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    

    // atributos publicos a otros scripts 
    
    public int modoSeleccionado;
    public void GetDropdownValue()

    {
        int indice = dropdown.value;
        string opcion = dropdown.options[indice].text;
        modoSeleccionado = indice;
        Debug.Log(opcion+":"+indice); // se muestra en la consola para debug 
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
