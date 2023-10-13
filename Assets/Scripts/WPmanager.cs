using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definição de uma estrutura chamada Link, que representa conexões entre waypoints
[System.Serializable]
public struct Link
{ 
    public enum direction {UNI, BI}; // Enumeração para representar direções de conexão
    public GameObject no1; // Primeiro ponto de ligação
    public GameObject no2; // Segundo ponto de ligação
    public direction dir; // Direção da conexão (unidirecional ou bidirecional)
}

public class WPmanager : MonoBehaviour
{
    int cont = 1; // Contador de waypoints
    int contlinks = 0; // Contador de conexões
    bool contclick = false; // Indicador de clique
    public GameObject point; // Prefab de waypoint
    public GameObject[] waypoint; // Array de waypoints
    public Link[] links; // Array de conexões
    public Grafo grafo = new Grafo(); // Instância de um grafo

    void Start()
    {
        // Método Start: Inicialização
    }

    // Função para adicionar um ponto (waypoint)
    void AdicionarPonto()
    {
        Vector3 mouse = Input.mousePosition;
        Vector3 pos = Camera.main.ScreenToWorldPoint(mouse);
        pos.z = 5;
        waypoint[cont] = Instantiate(point, pos, Quaternion.identity) as GameObject;
        AdicionarLink(cont);
        cont++;
    }

    // Função para configurar waypoints
    void Wp()
    {
        foreach (GameObject wp in waypoint)
        {
            grafo.AddNo(wp);
        }
        foreach (Link l in links)
        {
            grafo.addEdge(l.no1, l.no2);
            if (l.dir == Link.direction.BI)
            {
                grafo.addEdge(l.no2, l.no1);
            }
        }
    }

    // Função para adicionar uma conexão entre waypoints
    void AdicionarLink(int contpoint)
    {
        links[contlinks].no2 = waypoint[contpoint];
        links[contlinks].no1 = waypoint[contpoint - 1];
        links[contlinks].dir = Link.direction.BI;
        contlinks++;
    }

    // Função para adicionar a segunda conexão entre waypoints
    void AdicionarSegundoLink(int j)
    {
        if (contclick == false)
        {
            links[contlinks].no1 = waypoint[j];
            contclick = true;
            return;
        }
        else
        {
            links[contlinks].no2 = waypoint[j];
            contclick = false;
            links[contlinks].dir = Link.direction.BI;
            contlinks++;
            return;
        }
    }

    void Update()
    {
        if (cont < 5){
            if (Input.GetMouseButtonDown(0)){
                AdicionarPonto();
            }
        }
        if (cont == 5)
        {
            Wp();
            cont++;
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("ponto"))
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (waypoint[j] == hit.collider.gameObject)
                        {
                            AdicionarSegundoLink(j);
                        }
                    }
                }
            }
        }
    }
}
