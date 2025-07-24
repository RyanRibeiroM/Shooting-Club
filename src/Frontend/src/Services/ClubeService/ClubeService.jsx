import { apiFetch } from "../Api/Api";

export function cadastrarClube(dadosClube) {
  return apiFetch("/clube", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(dadosClube),
  });
}

export function getClube() {
  return apiFetch("/clube", {
    method: "GET",
  });
}

export function editarClube(dadosClube) {
  return apiFetch("/clube", {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(dadosClube),
  });
}
