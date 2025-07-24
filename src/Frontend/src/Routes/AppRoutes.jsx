import Home from "../Pages/Home/Home";
import Clube from "../Pages/Clube/Clube";
import Usuario from "../Pages/Usuario/Usuario";
import Armas from "../Pages/Armas/Armas";
import Associados from "../Components/Modal/Cadastro/Associados/Associados";
import Habitualidades from "../Components/Modal/Cadastro/Habitualidades/Habitualidades";
import Acervo from "../Components/Modal/Cadastro/Acervo/Acervo";
import Emprestimo from "../Pages/Emprestimo/Emprestimo";
import Itens from "../Components/Modal/Cadastro/Itens/Itens";
import Perfil from "../Pages/Perfil/Perfil";
import R_Habitualidades from "../Pages/Relatorio/R_Habitualidades/R_Habitualidades";
import Vencimentos from "../Pages/Relatorio/Vencimentos/Vencimentos";

const AppRoutes = [
  { path: "/home", element: <Home /> },
  { path: "/clube", element: <Clube /> },
  { path: "/usuario", element: <Usuario /> },
  { path: "/armas", element: <Armas /> },
  { path: "/associados", element: <Associados /> },
  { path: "/habitualidades", element: <Habitualidades /> },
  { path: "/acervo", element: <Acervo /> },
  { path: "/emprestimo", element: <Emprestimo /> },
  { path: "/itens", element: <Itens /> },
  { path: "/perfil", element: <Perfil /> },
  { path: "/r_habitualidades", element: <R_Habitualidades /> },
  { path: "/vencimentos", element: <Vencimentos /> },
];

export default AppRoutes;
