using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] List<InteractionEvent> interactionEvents;

    [SerializeField] float eventTimerInterval = 30f;

    float timer = 0;

    void Update()
    {
        // execute once every 30 seconds
        if (timer > eventTimerInterval)
        {
            timer = 0;
            InteractBasedOnEventRarity();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    void InteractBasedOnEventRarity()
    {
        foreach (InteractionEvent interactionEvent in interactionEvents)
        {
            switch (interactionEvent.eventRarity)
            {
                case EventRarity.Common:
                    if (Random.Range(0, 100) < 100)
                    {
                        DecideInteraction(interactionEvent);
                    }
                    break;
                case EventRarity.Uncommon:
                    if (Random.Range(0, 100) < 25)
                    {
                        DecideInteraction(interactionEvent);
                    }
                    break;
                case EventRarity.Rare:
                    if (Random.Range(0, 100) < 10)
                    {
                        DecideInteraction(interactionEvent);
                    }
                    break;
                case EventRarity.VeryRare:
                    if (Random.Range(0, 100) < 5)
                    {
                        DecideInteraction(interactionEvent);
                    }
                    break;
                case EventRarity.Legendary:
                    if (Random.Range(0, 100) < 1)
                    {
                        DecideInteraction(interactionEvent);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void DecideInteraction(InteractionEvent interactionEvent)
    {
        if (!interactionEvent.isPlaying && interactionEvent.CheckPlayable())
        {
            interactionEvent.PlayInteraction();
        }
    }
}