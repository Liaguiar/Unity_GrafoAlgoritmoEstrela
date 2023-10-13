using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esta classe representa uma conexão (aresta) entre nós (waypoints) no grafo
public class Edge
{
    public No noInicial; // Nó de origem da aresta
    public No noFinal;   // Nó de destino da aresta

    public Edge(No inicio, No fim)
    {
        noInicial = inicio;
        noFinal = fim;
    }
}
