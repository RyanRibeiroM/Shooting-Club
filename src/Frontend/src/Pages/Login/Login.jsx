import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { FaUser, FaLock, FaSignInAlt } from "react-icons/fa";
import { login as loginService } from "../../Services/AuthService/AuthService";
import { useAuth } from "../../Hooks/useAuth/useAuth";
import "./Login.css";

const Login = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(false);
  const [errors, setErrors] = useState({ email: "", password: "" });

  const navigate = useNavigate();
  const { login } = useAuth();

  const handleSubmit = async (event) => {
    event.preventDefault();

    const newErrors = {
      email: !username.includes("@") ? "Insira um e-mail válido." : "",
      password: password.length < 4 ? "A senha deve ter pelo menos 4 caracteres." : "",
    };

    setErrors(newErrors);

    if (newErrors.email || newErrors.password) return;

    try {
      const data = await loginService(username, password);
      const accessToken = data.tokens?.accessToken;
      const refreshToken = data.tokens?.refreshToken;

      login(accessToken, refreshToken, rememberMe);
      navigate("/home");
    } catch (error) {
      setErrors({
        email: "",
        password: "E-mail ou senha incorretos.",
      });
    }
  };

  return (
    <div className="login-wrapper">
      <div className="container">
        <form onSubmit={handleSubmit} noValidate>
          <div className="logo-header">
            <img src="/logo.jpeg" alt="Logo" className="logo-image" />
            <div className="logo-text-block">
              <h2 className="logo-title">
                CLUBE DE TIRO
                <br />
                ESPORTIVO DE CRATEÚS
              </h2>
            </div>
          </div>

          <h2 className="text-xl font-semibold text-black">Inscreva-se</h2>
          <p className="text-sm text-gray-600">
            Entre com o e-mail e senha para acessar a conta
          </p>

          <div className="input-field">
            <input
              type="email"
              placeholder="E-mail"
              className={errors.email ? "input-error" : ""}
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
            <FaUser className="icon" />
            {errors.email && <span className="error-message">{errors.email}</span>}
          </div>

          <div className="input-field">
            <input
              type="password"
              placeholder="Senha"
              className={errors.password ? "input-error" : ""}
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
            <FaLock className="icon" />
            {errors.password && <span className="error-message">{errors.password}</span>}
          </div>

          <div className="recall-forget">
            <label>
              <input
                type="checkbox"
                checked={rememberMe}
                onChange={(e) => setRememberMe(e.target.checked)}
              />
              Lembre de mim
            </label>
            <a href="#">Esqueceu sua senha?</a>
          </div>

          <button type="submit">
            <span className="button-text">Entrar</span>
            <FaSignInAlt className="button-icon" />
          </button>
        </form>
      </div>

      <div className="login-footer">
        <p>2025 © Projeto desenvolvido por <strong>CTRVF</strong></p>
      </div>
    </div>
  );
};

export default Login;
