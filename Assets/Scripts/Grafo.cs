using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definição de uma classe chamada "Grafo" que representa um grafo de conexões entre nós
public class Grafo
{
    List<Edge> edges = new List<Edge>(); // Lista de arestas no grafo
    List<No> nos = new List<No>(); // Lista de nós no grafo
    public List<No> pathLista = new List<No>(); // Lista de nós que compõem o caminho

    public Grafo() { }

    public void AddNo(GameObject id)
    {
        // Adiciona um nó (waypoint) ao grafo com base em um objeto GameObject
        No no = new No(id);
        nos.Add(no);
    }

    public void addEdge(GameObject noinicial, GameObject noFinal)
    {
        // Adiciona uma aresta (conexão) entre dois nós (waypoints) no grafo
        No inicio = EncontraNo(noinicial);
        No fim = EncontraNo(noFinal);
        if (inicio != null && fim != null)
        {
            Edge e = new Edge(inicio, fim);
            edges.Add(e);
            inicio.edgesLista.Add(e);
        }
    }

    No EncontraNo(GameObject id)
    {
        // Encontra um nó (waypoint) no grafo com base em um objeto GameObject
        foreach (No n in nos)
        {
            if (n.getId() == id)
            {
                return n;
            }
        }
        return null;
    }
    public bool Estrela(GameObject inicialId, GameObject finalId)
    {
        // Método que implementa o algoritmo A* (Estrela) para encontrar o caminho entre dois nós (waypoints)

        No inicial = EncontraNo(inicialId);
        No final = EncontraNo(finalId);

        if (inicial == null || final == null)
        {
            // Se o nó de início ou o nó de destino não forem encontrados, retorna falso (não há caminho)
            return false;
        }

        List<No> open = new List<No>();
        List<No> close = new List<No>();
        float tentative_g_score = 0;
        bool tentative_is_better;

        inicial.g = 0;
        inicial.h = distancia(inicial, final);
        inicial.f = inicial.h;
        open.Add(inicial);

        while (open.Count > 0)
        {
            int i = menor(open);
            No thisNo = open[i];

            if (thisNo.getId() == finalId)
            {
                // Se o nó atual for o nó de destino, reconstrói o caminho e retorna verdadeiro (caminho encontrado)
                RecontrutorPath(inicial, final);
                return true;
            }

            open.RemoveAt(i);
            close.Add(thisNo);

            No vizinho;
            foreach (Edge e in thisNo.edgesLista)
            {
                vizinho = e.noFinal;

                if (close.IndexOf(vizinho) > -1)
                {
                    continue; // O vizinho já está na lista fechada, então continue para o próximo vizinho
                }

                tentative_g_score = thisNo.g + distancia(thisNo, vizinho);

                if (open.IndexOf(vizinho) == -1)
                {
                    open.Add(vizinho);
                    tentative_is_better = true;
                }
                else if (tentative_g_score < vizinho.g)
                {
                    tentative_is_better = true;
                }
                else
                {
                    tentative_is_better = false;
                }

                if (tentative_is_better)
                {
                    // Atualiza os valores do vizinho com base na tentativa atual
                    vizinho.origem = thisNo;
                    vizinho.g = tentative_g_score;
                    vizinho.h = distancia(thisNo, final);
                    vizinho.f = vizinho.g + vizinho.h;
                }
            }
        }

        // Se o loop terminar e não encontrar um caminho, retorna falso
        return false;
    }
    public void RecontrutorPath(No inicioId, No finalId)
    {
        // Método para reconstruir o caminho a ser seguido a partir do nó de origem ao nó de destino

        pathLista.Clear(); // Limpa a lista de nós do caminho
        pathLista.Add(finalId); // Adiciona o nó de destino à lista de caminho
        var p = finalId.origem;

        while (p != inicioId && p != null)
        {
            // Enquanto não alcançamos o nó de origem ou não chegamos ao final do caminho, continue
            pathLista.Insert(0, p); // Insere o nó atual no início da lista de caminho
            p = p.origem; // Move para o próximo nó no caminho
        }

        pathLista.Insert(0, inicioId); // Adiciona o nó de origem ao início da lista de caminho
    }

    float distancia(No a, No b)
    {
        // Método para calcular a distância (custo) entre dois nós (waypoints)

        return (Vector3.SqrMagnitude(a.getId().transform.position - b.getId().transform.position));
    }

    int menor(List<No> l)
    {
        // Método para encontrar o índice do nó com o menor custo (f) em uma lista de nós

        float menorf = 0;
        int count = 0;
        int interatoCount = 0;
        menorf = l[0].f;

        for (int i = 1; i < l.Count; i++)
        {
            if (l[i].f <= menorf)
            {
                menorf = l[i].f;
                interatoCount = count;
            }
            count++;
        }

        return interatoCount; // Retorna o índice do nó com o menor custo na lista
    }
}
