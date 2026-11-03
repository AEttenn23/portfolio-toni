// EnemyAiBase.cs (extracto) - la IA decide cada 0.5 s: patrullar, perseguir o atacar
    private IEnumerator AIUpdateROutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(aiUpdateFrequency);

            targetInSightRange = useSightRange && IsTargetInSight();
            targetInAttackRange = Vector3.Distance(EyePosition, target.position) <= attackRange;


            if (targetInSightRange && targetInAttackRange)
                AttackTarget();
            else if (targetInSightRange && !targetInAttackRange)
                ChaseTarget();
            else
                Patrolling();
        }
    }

    private Vector3 EyePosition => enemyCollider.bounds.center;

    private bool IsTargetInSight()
    {
        if (target == null || enemyCollider == null)
            return false;

        Vector3 origin = EyePosition;

        Collider targetCol = target.GetComponent<Collider>();
        Vector3 targetCenter = targetCol != null
            ? targetCol.bounds.center
            : target.position;

        Vector3 dir = (targetCenter - origin).normalized;
        float distance = Vector3.Distance(origin, targetCenter);

        // USAR sightRange (no viewDistance)
        if (distance > sightRange)
            return false;

        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > viewAngle * 0.5f)
            return false;

        // Raycast SOLO contra obstáculos + player
        if (Physics.Raycast(origin, dir, out RaycastHit hit, sightRange,
            obstacleMask | targetLayer))
        {
            if (hit.collider.transform == target ||
                hit.collider.transform.IsChildOf(target))
            {
                return true;
            }
        }

        return false;
    }
