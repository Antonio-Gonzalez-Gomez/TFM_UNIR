using System.Collections.Generic;
using UnityEngine;

public class DragAugmentSpot : MonoBehaviour
{
    [SerializeField] public int maxAugments;
    [SerializeField] public float spotHeight;
    [SerializeField] InfoPopUp cardInfoPopUp;
    [SerializeField] ScoreUIController uiController;
    [SerializeField] AugmentInstance augmentPrefab;

    public List<AugmentInstance> augmentList;
    private List<Vector3> augmentPositions;

    private void Awake()
    {
        augmentList = new List<AugmentInstance>();
        augmentPositions = new List<Vector3>();
        //Las posiciones de los aumentos son estaticas, por lo que se calculan aqui una vez
        for (int i = 0; i < maxAugments; i++)
        {
            //Para evitar que los aumentos se queden en los extremos (sin centrarse), se intercalan los indices
            float halfInd = i + 0.5f;
            //Partiendo de la posicion (centro del spot), se reparte la distancia a partir del numero MAXIMO de aumentos
            //Esto hace que las posiciones que pueden ocupar sean estaticas (dejando huecos vacios visibles hacia abajo)
            float incY = spotHeight * halfInd / maxAugments - spotHeight / 2;
            //Si se quisiera hacer dinamico (como las cartas en mano), usar augmentList.Count en lugar de maxAugments
            augmentPositions.Add(this.transform.position + new Vector3(0, incY, 0));
        }
        //Se le da la vuelta a la lista para empezar con las posiciones superiores
        augmentPositions.Reverse();
    }

    //TODO: Reordenacion aumentos

    private void UpdateAugmentsPositions()
    {
        for (int i = 0; i < augmentList.Count; i++)
        {
            DragController drag = augmentList[i].drag;
            drag.originalPosition = augmentPositions[i];
            drag.originalRotation = Quaternion.identity;
            drag.ResetPosition();
        }
    }

    //Metodo que crea un aumento nuevo y lo enlaza a este spot (si queda hueco)
    public void AddAugment(AugmentData data)
    {        
        //Se comprueba que quede espacio
        if (augmentList.Count >= maxAugments)
            return;

        AugmentInstance newAug = Instantiate(augmentPrefab);
        newAug.SetAugment(data);
        DragController drag = newAug.GetComponent<DragController>();
        //Los eventos usados para enseñar/ocultar informacion del aumento
        //Deben iniciarse desde el controlador de UI haciendo referencia al DragController
        cardInfoPopUp.ConnectEvents(drag);
        //Lo mismo para los eventos de los botones
        uiController.ConnectEvents(drag);
        augmentList.Add(newAug);
        newAug.drag.SetSpot(this);

        UpdateAugmentsPositions();
    }

    public void RemoveAugment(AugmentInstance removedAug)
    {
        augmentList.Remove(removedAug);
        UpdateAugmentsPositions();
    }
}
