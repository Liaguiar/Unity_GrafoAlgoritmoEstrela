using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirWP : MonoBehaviour
{
    Transform goal; // Ponto de destino (waypoint) atual
    float speed = 5.0f; // Velocidade de movimento do objeto
    float accuracy = 5.0f; // Precisão para alcançar o waypoint
    float rotSpeed = 2.0f; // Velocidade de rotação do objeto
    public GameObject WPmanager; // Gerenciador de waypoints
    GameObject[] wps; // Array de waypoints
    GameObject currentNode; // Waypoint atual
    int currentWP = 0; // Índice do waypoint atual
    Grafo g; // Instância de um grafo

    void Start()
    {
        // Inicialização: Obtém os waypoints e o grafo do gerenciador de waypoints
        wps = WPmanager.GetComponent<WPmanager>().waypoint;
        g = WPmanager.GetComponent<WPmanager>().grafo;
        currentNode = wps[0]; // Inicializa o waypoint atual como o primeiro da lista
    }

    void LateUpdate()
    {
        // Verifica se a lista de waypoints está vazia ou se já alcançamos o último waypoint
        if (g.pathLista.Count == 0 || currentWP == g.pathLista.Count)
        {
            return;
        }

        // Verifica se a distância entre o objeto e o waypoint atual é menor que a precisão definida
        if (Vector3.Distance(g.pathLista[currentWP].getId().transform.position, this.transform.position) < accuracy)
        {
            currentNode = g.pathLista[currentWP].getId();
            currentWP++;
        }

        // Move o objeto em direção ao próximo waypoint
        if (currentWP < g.pathLista.Count)
        {
            goal = g.pathLista[currentWP].getId().transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x, goal.position.y, 4);
            Vector3 direction = lookAtGoal - this.transform.position;
            this.transform.Translate(0, 0, speed * Time.deltaTime);

            // Realiza a rotação do objeto para olhar na direção do próximo waypoint
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);
        }
    }

    void Update()
    {
        // Verifica se o botão do mouse do meio (2) foi pressionado
        if (Input.GetMouseButtonDown(2)) {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Realiza um raio (raycast) para detectar colisões com objetos
            if (Physics.Raycast(ray, out hit, 10.0f))
            {
                if (hit.collider.tag == "ponto") // Verifica se o objeto colidido tem a tag "ponto"
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (WPmanager.GetComponent<WPmanager>().waypoint[i] == hit.collider.gameObject)
                        {
                            g.Estrela(currentNode, wps[i]); // Aplica o algoritmo A* (Estrela) para encontrar o caminho
                            currentWP = 0; // Reinicializa o índice do waypoint atual
                        }
                    }
                }
            }
        }
    }
}
