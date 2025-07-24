import React, { createContext, useState, useEffect } from "react";

export const AuthContext = createContext(); 

export function AuthProvider({ children }) {
  const [token, setToken] = useState(localStorage.getItem("token") || null);
  const [refreshToken, setRefreshToken] = useState(localStorage.getItem("refreshToken") || null);
  const [user, setUser] = useState(null);

  useEffect(() => {
    if (token) {
      localStorage.setItem("token", token);
    } else {
      localStorage.removeItem("token");
      setUser(null);
    }
  }, [token]);

  useEffect(() => {
    if (refreshToken) {
      localStorage.setItem("refreshToken", refreshToken);
    } else {
      localStorage.removeItem("refreshToken");
    }
  }, [refreshToken]);

  const login = (newToken, newRefreshToken, rememberMe) => {
    setToken(newToken);
    if (rememberMe) {
      setRefreshToken(newRefreshToken);
    }
  };

  const logout = () => {
    setToken(null);
    setRefreshToken(null);
  };

  return (
    <AuthContext.Provider value={{ token, refreshToken, user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}
