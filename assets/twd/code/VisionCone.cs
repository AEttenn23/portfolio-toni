// VisionCone.cs (extracto) - malla del cono generada en runtime y color según el estado de la IA
        Color coneColor;

        if (enemyAiBase.isFillingBar)
        {
            viewDistance = fillingViewDistance;
            coneColor = colorFilling;
        }
        else if (enemyAiBase.targetInSightRange && !enemyAiBase.targetInAttackRange)
        {
            viewDistance = chasingViewDistance;
            coneColor = colorChasing;
        }
        else
        {
            viewDistance = chasingViewDistance;
            coneColor = colorPatrolling;
        }


        // Asignar color al material
        rend.material.SetColor("_BaseColor", coneColor);

    }

    void GenerateConeMesh()
    {
        coneMesh.Clear();

        Vector3 origin = new Vector3(0, 0.05f, 0); // ahora el cono se genera en local space
        Vector3[] vertices = new Vector3[segmentCount + 2];
        int[] triangles = new int[segmentCount * 3];

        vertices[0] = origin;

        float halfAngle = viewAngle / 2f;

        for (int i = 0; i <= segmentCount; i++)
        {
            float angle = -halfAngle + (viewAngle / segmentCount) * i;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward; // usa forward local
            vertices[i + 1] = origin + dir * viewDistance;
        }

        for (int i = 0; i < segmentCount; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        coneMesh.vertices = vertices;
        coneMesh.triangles = triangles;
        coneMesh.RecalculateNormals();
    }

