import { apiFetch } from "../Api/Api";

export function login(email, senha) {
  return apiFetch("/login", {
    method: "POST",
    body: JSON.stringify({ email, senha }),
  });
}

export function logout() {
  localStorage.removeItem("token");
}

export function refreshToken(refreshToken) {
  return apiFetch("/refresh-token", {
    method: "POST",
    body: JSON.stringify({ refreshToken }),
  });
}
