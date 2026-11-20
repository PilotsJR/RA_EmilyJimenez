using System.Collections.Generic;
using UnityEngine;

public enum TipoModelo { A, B, C, D }

public class ModeloInteractivo : MonoBehaviour
{
    private enum Relacion { Ninguna, Atacar, Bailar }

    private static readonly List<ModeloInteractivo> Activos = new List<ModeloInteractivo>();

    [SerializeField] private TipoModelo tipo;
    [SerializeField] private Animator animator;

    [Header("Distancias (metros)")]
    [SerializeField] private float distanciaActivar = 0.15f;   
    [SerializeField] private float distanciaDesactivar = 0.20f; 

    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int DanceHash = Animator.StringToHash("Dance");

    private bool atacando;
    private bool bailando;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        Activos.Add(this);
    }

    private void OnDisable()
    {
        Activos.Remove(this);
        atacando = false;
        bailando = false;
    }

    private void Update()
    {
        ModeloInteractivo masCercano = null;
        Relacion relacionCercana = Relacion.Ninguna;
        float mejorDistancia = float.MaxValue;

        foreach (var otro in Activos)
        {
            if (otro == this) continue;

            Relacion rel = ObtenerRelacion(tipo, otro.tipo);
            if (rel == Relacion.Ninguna) continue;

            float dist = Vector3.Distance(transform.position, otro.transform.position);
            if (dist < mejorDistancia)
            {
                mejorDistancia = dist;
                masCercano = otro;
                relacionCercana = rel;
            }
        }

        bool yaInteractuando = atacando || bailando;
        float umbral = yaInteractuando ? distanciaDesactivar : distanciaActivar;

        bool cerca = masCercano != null && mejorDistancia <= umbral;

        bool nuevoAttack = cerca && relacionCercana == Relacion.Atacar;
        bool nuevoDance = cerca && relacionCercana == Relacion.Bailar;

        if (nuevoAttack != atacando)
        {
            atacando = nuevoAttack;
            animator.SetBool(AttackHash, atacando);
        }

        if (nuevoDance != bailando)
        {
            bailando = nuevoDance;
            animator.SetBool(DanceHash, bailando);
        }
    }

    private static Relacion ObtenerRelacion(TipoModelo a, TipoModelo b)
    {
        if (Es(a, b, TipoModelo.A, TipoModelo.B)) return Relacion.Atacar;
        if (Es(a, b, TipoModelo.C, TipoModelo.D)) return Relacion.Atacar;
        if (Es(a, b, TipoModelo.A, TipoModelo.C)) return Relacion.Bailar;
        if (Es(a, b, TipoModelo.B, TipoModelo.D)) return Relacion.Bailar;
        return Relacion.Ninguna;
    }

    private static bool Es(TipoModelo a, TipoModelo b, TipoModelo x, TipoModelo y)
    {
        return (a == x && b == y) || (a == y && b == x);
    }
}