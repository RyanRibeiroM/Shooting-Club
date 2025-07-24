import React, { useState } from "react";
import InputMask from "react-input-mask";
import { editarArma } from "../../../../Services/ArmasService/ArmasService";
import { ParseFormErrorsGun } from "../../../Utils/ParseFormErrors/ParseFormErrorsGun";
import FormErrors from "../../../Utils/FormErrors/FormErrors";

const camposBase = [
  { name: "tipo", label: "Tipo de Arma", type: "text" },
  { name: "marca", label: "Marca", type: "text" },
  { name: "calibre", label: "Calibre", type: "text" },
  { name: "numeroSerie", label: "Número de Série", type: "text" },
  {
    name: "tipoPosse",
    label: "Tipo de Posse",
    type: "select",
    options: [
      { value: "", label: "Selecione..." },
      { value: "Exército", label: "Exército" },
      { value: "Polícia Federal", label: "Polícia Federal" },
      { value: "PortePessoal", label: "Porte Pessoal" },
    ],
    disabled: true,
  },
  { name: "cpf_proprietario", label: "CPF do Proprietário", type: "text", mask: "999.999.999-99" },
];

const camposPorPosse = {
  Exército: [
    { name: "numeroSigma", label: "Número SIGMA", type: "text" },
    { name: "dataExpedicaoCRAF", label: "Data Expedição CRAF", type: "date" },
    { name: "validadeCRAF", label: "Validade CRAF", type: "date" },
    { name: "localRegistro", label: "Local de Registro", type: "text" },
    { name: "numeroGTE", label: "Número GTE", type: "text" },
    { name: "validadeGTE", label: "Validade GTE", type: "date" },
  ],
  "Polícia Federal": [
    { name: "numeroRegistroPF", label: "Número Registro PF", type: "text" },
    { name: "numeroSinarm", label: "Número SINARM", type: "text" },
    { name: "numeroNotaFiscal", label: "Número Nota Fiscal", type: "text" },
    { name: "dataValidadePF", label: "Data Validade PF", type: "date" },
  ],
  PortePessoal: [
    { name: "numeroCertificado", label: "Número Certificado", type: "text" },
    { name: "validadeCertificacao", label: "Validade Certificação", type: "date" },
  ],
};

const tipoPosseStringParaNumero = (str) => {
  switch (str) {
    case "Exército":
      return 0;
    case "Polícia Federal":
      return 1;
    case "PortePessoal":
      return 2;
    default:
      return null;
  }
};

const EditarArmasModal = ({ isOpen, onClose, form, handleChange, onSubmit }) => {
  const [errors, setErrors] = useState(null);

  if (!isOpen) return null;

  const handleOverlayClick = (e) => {
    if (e.target.classList.contains("modal-overlay")) onClose();
  };

  const validarCampos = () => {
    const newErrors = {};

    camposBase.forEach(({ name }) => {
      if (!form[name] || form[name].toString().trim() === "") {
        newErrors[name] = ["Campo obrigatório"];
      }
    });

    if (form.tipoPosse && camposPorPosse[form.tipoPosse]) {
      camposPorPosse[form.tipoPosse].forEach(({ name }) => {
        if (!form[name] || form[name].toString().trim() === "") {
          newErrors[name] = ["Campo obrigatório"];
        }
      });
    }

    return newErrors;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErrors(null);

    const errosValidacao = validarCampos();

    if (Object.keys(errosValidacao).length > 0) {
      setErrors(errosValidacao);
      return;
    }

    const tipoPosseNum = tipoPosseStringParaNumero(form.tipoPosse);
    if (tipoPosseNum === null) {
      setErrors({ geral: ["Tipo de posse inválido!"] });
      return;
    }

    const payloadBase = {
      tipoPosse: tipoPosseNum,
      cpf_proprietario: form.cpf_proprietario,
      tipo: form.tipo,
      marca: form.marca,
      calibre: form.calibre,
      numeroSerie: form.numeroSerie,
    };

    let payloadEspecifico = {};

    switch (form.tipoPosse) {
      case "Exército":
        payloadEspecifico = {
          numeroSigma: form.numeroSigma,
          dataExpedicaoCRAF: form.dataExpedicaoCRAF,
          validadeCRAF: form.validadeCRAF,
          localRegistro: form.localRegistro,
          numeroGTE: form.numeroGTE,
          validadeGTE: form.validadeGTE,
        };
        break;
      case "Polícia Federal":
        payloadEspecifico = {
          numeroRegistroPF: form.numeroRegistroPF,
          numeroSinarm: form.numeroSinarm,
          numeroNotaFiscal: form.numeroNotaFiscal,
          dataValidadePF: form.dataValidadePF,
        };
        break;
      case "PortePessoal":
        payloadEspecifico = {
          numeroCertificado: form.numeroCertificado,
          validadeCertificacao: form.validadeCertificacao,
        };
        break;
      default:
        setErrors({ geral: ["Tipo de posse inválido!"] });
        return;
    }

    const payload = {
      ...payloadBase,
      ...payloadEspecifico,
    };

    try {
      await editarArma(form.id, payload);
      alert("Arma editada com sucesso!");
      onSubmit?.();
      onClose();
    } catch (error) {
      const parsedErrors = ParseFormErrorsGun(error);
      setErrors(parsedErrors);
    }
  };

  const getInputClass = (field) => (errors?.[field] ? "input-error" : "");

  return (
    <div className="cadastro-armas-modal">
      <div className="modal-overlay" onClick={handleOverlayClick}>
        <div className="modal-box">
          <h2>Editar Arma</h2>
          <p>Atualize as informações da arma</p>

          {/* Exibe erros gerais */}
          <FormErrors errors={errors} field="geral" />

          <form className="modal-form" onSubmit={handleSubmit}>
            <div className="form-row">
              {camposBase.map(({ name, label, type = "text", mask, options, disabled }) => {
                if (type === "select") {
                  return (
                    <div className="form-group" key={name}>
                      <label>{label}</label>
                      <select
                        name={name}
                        value={form[name] || ""}
                        onChange={handleChange}
                        disabled={disabled}
                        className={getInputClass(name)}
                      >
                        {options.map(({ value, label }) => (
                          <option key={value} value={value}>{label}</option>
                        ))}
                      </select>
                      <FormErrors errors={errors} field={name} />
                    </div>
                  );
                }

                return (
                  <div className="form-group" key={name}>
                    <label>{label}</label>
                    {mask ? (
                      <InputMask
                        mask={mask}
                        name={name}
                        value={form[name] || ""}
                        onChange={handleChange}
                        className={getInputClass(name)}
                      >
                        {(inputProps) => <input {...inputProps} type={type} />}
                      </InputMask>
                    ) : (
                      <input
                        type={type}
                        name={name}
                        value={form[name] || ""}
                        onChange={handleChange}
                        className={getInputClass(name)}
                      />
                    )}
                    <FormErrors errors={errors} field={name} />
                  </div>
                );
              })}
            </div>

            {form.tipoPosse && camposPorPosse[form.tipoPosse] && (
              <div className="form-row">
                {camposPorPosse[form.tipoPosse].map(({ name, label, type = "text", mask }) => (
                  <div className="form-group" key={name}>
                    <label>{label}</label>
                    {mask ? (
                      <InputMask
                        mask={mask}
                        name={name}
                        value={form[name] || ""}
                        onChange={handleChange}
                        className={getInputClass(name)}
                      >
                        {(inputProps) => <input {...inputProps} type={type} />}
                      </InputMask>
                    ) : (
                      <input
                        type={type}
                        name={name}
                        value={form[name] || ""}
                        onChange={handleChange}
                        className={getInputClass(name)}
                      />
                    )}
                    <FormErrors errors={errors} field={name} />
                  </div>
                ))}
              </div>
            )}

            <div className="modal-buttons">
              <button type="submit" className="btn-submit">Salvar Alterações</button>
              <button type="button" className="btn-cancel" onClick={onClose}>Cancelar</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default EditarArmasModal;
