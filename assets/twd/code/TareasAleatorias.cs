// TareasAleatorias.cs (extracto) - genera la lista de tareas de cada partida
    public void GenerarListaTareas()
    {
        if (PrefabsTareas == null || PrefabsTareas.Length == 0)
        {
            Debug.LogError("PrefabsTareas esta vacio");
            return;
        }

        List<GameObject> tareasDisponibles = new List<GameObject>(PrefabsTareas);

        OrdenTareas.Clear();
        tareaToUI.Clear();

        // Agregar tareas obligatorias
        foreach (GameObject tarea in tareasObligatorias)
        {
            if (tarea != null && !OrdenTareas.Contains(tarea))
            {
                OrdenTareas.Add(tarea);
                tareasDisponibles.Remove(tarea);
            }
        }

        // Uwe opcional
        bool incluirUwe = (Uwe != null && Random.value < 0.2f);

        if (incluirUwe && !OrdenTareas.Contains(Uwe))
        {
            OrdenTareas.Add(Uwe);
            tareasDisponibles.Remove(Uwe);
        }

        // Tareas aleatorias restantes
        MezclarLista(tareasDisponibles);

        int restantes = TareasPorNivel - OrdenTareas.Count;

        for (int i = 0; i < restantes && i < tareasDisponibles.Count; i++)
        {
            OrdenTareas.Add(tareasDisponibles[i]);
        }

    // ... crea la UI de cada tarea
}
