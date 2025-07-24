export const ParseFormErrors = (error) => {
  const fieldErrors = {};

  const addFieldError = (field, msg) => {
    fieldErrors[field] = (fieldErrors[field] || []).concat(msg.trim());
  };

  const removeAccents = (str) =>
    str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");

  try {
    const errorData = JSON.parse(error.message);

    if (Array.isArray(errorData.errors)) {
      errorData.errors.forEach((msg) => {
        const normalized = removeAccents(msg.toLowerCase().trim());

        if (normalized.includes("data de renovacao da filiacao")) {
          addFieldError("dataRenovacaoFiliacao", msg);
        } else if (
          normalized.includes("data da filiacao") ||
          (normalized.includes("data") &&
            normalized.includes("filiacao") &&
            !normalized.includes("renovacao"))
        ) {
          addFieldError("dataFiliacao", msg);
        } else if (normalized.includes("numero de filiacao")) {
          addFieldError("numeroFiliacao", msg);
        } else if (normalized.includes("nome")) {
          addFieldError("nome", msg);
        } else if (normalized.includes("email")) {
          addFieldError("email", msg);
        } else if (normalized.includes("senha")) {
          addFieldError("senha", msg);
        } else if (normalized.includes("cpf")) {
          addFieldError("cpf", msg);
        } else if (normalized.includes("nascimento")) {
          addFieldError("dataNascimento", msg);
        } else if (normalized.includes("pais")) {
          addFieldError("enderecoPais", msg);
        } else if (normalized.includes("estado")) {
          addFieldError("enderecoEstado", msg);
        } else if (normalized.includes("cidade")) {
          addFieldError("enderecoCidade", msg);
        } else if (normalized.includes("bairro")) {
          addFieldError("enderecoBairro", msg);
        } else if (normalized.includes("rua")) {
          addFieldError("enderecoRua", msg);
        } else if (normalized.includes("numero") && normalized.includes("endereco")) {
          addFieldError("enderecoNumero", msg);
        } else if (normalized.includes("validade") || normalized.includes("vencimento")) {
          addFieldError("dataVencimentoCR", msg);
        } else if (normalized.includes("cr")) {
          addFieldError("cr", msg);
        } else if (normalized.includes("sfpc")) {
          addFieldError("sfpcVinculacao", msg);
        } else {
          addFieldError("general", msg);
        }
      });
    } else {
      addFieldError("general", "Erro inesperado ao processar a resposta do servidor.");
    }
  } catch (e) {
    addFieldError("general", "Erro inesperado ao processar a resposta do servidor.");
  }

  return fieldErrors;
};
