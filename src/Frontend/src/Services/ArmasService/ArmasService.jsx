import { apiFetch } from "../Api/Api";

export function cadastrarArmas(dadosArmas) {
  return apiFetch("/arma", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(dadosArmas),
  });
}

export function editarArma(id, dadosArma) {
  return apiFetch(`/arma/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(dadosArma),
  });
}

export function filtrarArmas(filtros) {
  return apiFetch("/arma/filter", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(filtros),
  });
}

export function deletarArma(id) {
  return apiFetch(`/arma/${id}`, {
    method: "DELETE",
  });
}

export function buscarArmaPorId(id) {
  return apiFetch(`/arma/${id}`, {
    method: "GET",
  });
}
