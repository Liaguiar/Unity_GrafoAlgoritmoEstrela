using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definição de uma classe chamada "No" que representa um nó no grafo
public class No
{
    public List<Edge> edgesLista = new List<Edge>(); // Lista de arestas conectadas a este nó
    public No path = null; // Nó anterior no caminho
    GameObject id; // Referência ao objeto no Unity associado a este nó
    public float f, g, h; // Valores usados em algoritmos de busca (custos)
    public No origem; // Nó de origem, usado para rastreamento de caminho

    public No(GameObject i)
    {
        id = i; // Inicializa o nó com um objeto GameObject associado
        path = null; // Inicializa o nó anterior como nulo
    }

    public GameObject getId()
    {
        return id; // Retorna o objeto GameObject associado a este nó
    }
}
