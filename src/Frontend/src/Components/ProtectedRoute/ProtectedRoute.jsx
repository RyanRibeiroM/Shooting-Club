import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../../Context/AuthContext/AuthContext";

const ProtectedRoute = () => {
  // Pega os dados de autenticação do nosso contexto
  const { isAuthenticated, isLoading } = useAuth();

  // Se ainda estamos verificando se existe um token (no F5),
  // mostramos uma tela de "Carregando..." para evitar problemas.
  if (isLoading) {
    return <div>Carregando...</div>;
  }

  // Se não estiver autenticado, expulsa o usuário para a página de login.
  if (!isAuthenticated) {
    return <Navigate to="/" replace />;
  }

  // Se passou por todas as verificações, permite o acesso à página solicitada.
  // O <Outlet /> é o componente que o React Router usa para renderizar a rota filha.
  return <Outlet />;
};

export default ProtectedRoute;
