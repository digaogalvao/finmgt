import { Navmenu } from '../components/Nav/nav';
import { Api } from '../providers';
import { SetStateAction, useEffect, useState } from 'react';
import { Button, Col, Row, Table } from 'react-bootstrap';
import {Modal, ModalBody, ModalFooter, ModalHeader} from 'reactstrap';
import { format } from 'date-fns';
import { SelectTipo } from '../components/SelectTipo/SelectTipo';
import { DateTimePicker } from '../components/DateTimePicker/DateTimePicker';

export function LancamentoPage() {

  interface ILancamento {
    id?: any;
    data?: Date;
    descricao: string;
    valor: any;
    tipo?: any;
  }

  const [data, setData] = useState<ILancamento[]>([]);

  //Modals
  const [showInsert, setShowInsert] = useState(false);
  const [showEdit, setShowEdit] = useState(false);
  const [showDelete, setShowDelete] = useState(false);

  const [lancamentoSelecionado, setLancamentoSelecionado] = useState<ILancamento>({
    id: undefined,
    data: new Date(),
    descricao: '',
    valor: undefined,
    tipo: undefined,

  })

  const handleChange = (e: { target: { name: any; value: any; }; }) => {
    const { name, value } = e.target;
  
    if (name === 'data') {
      // Validar se o valor inserido é uma data válida antes de tentar convertê-lo
      const isValidDate = !isNaN(new Date(value).getTime());
  
      setLancamentoSelecionado({
        ...lancamentoSelecionado,
        [name]: isValidDate ? new Date(value) : undefined,
      });
    } else {
      setLancamentoSelecionado({
        ...lancamentoSelecionado,
        [name]: value,
      });
    }
  };

    //Modal controle do estado
    const showHideInsert = () => {
      setShowInsert(!showInsert);
    }
  
    const showHideEdit = () => {
      setShowEdit(!showEdit);
    }
  
     const showHideDelete = () => {
      setShowDelete(!showDelete);
    }

  //Lista lançamentos
  const lancamentoGet=async()=>{
    await Api.get("Transacoes")
    .then(response=>{
      setData(response.data);
    })
    .catch(error=>{
      console.log(error);
    })
  }

  //Insere lançamento
  const canalPost = async () => {
    try {
      const response = await Api.post("Transacoes", {
        ...lancamentoSelecionado,
        data: lancamentoSelecionado.data ? format(new Date(lancamentoSelecionado.data), 'yyyy-MM-dd') : null,
        valor: parseFloat(lancamentoSelecionado.valor),
        tipo: parseInt(lancamentoSelecionado.tipo),
      });
  
      setData(data.concat(response.data));
      showHideInsert();
      await lancamentoGet();
    } catch (error) {
      console.log(error);
    }
  };
  
  //Altera lançamento
  const canalPut = async () => {
    try {
      const response = await Api.put(`Transacoes/${lancamentoSelecionado.id}`, {
        id: lancamentoSelecionado.id,
        data: lancamentoSelecionado.data,
        descricao: lancamentoSelecionado.descricao,
        valor: parseFloat(lancamentoSelecionado.valor),
        tipo: parseInt(lancamentoSelecionado.tipo)
      });
  
      var resposta = response.data;
      var dadosAuxiliar = data;
      dadosAuxiliar.forEach(lancamento => {
        if (lancamento.id === lancamentoSelecionado.id) {
          lancamento.data = resposta.data;
          lancamento.descricao = resposta.descricao;
          lancamento.valor = resposta.valor;
          lancamento.tipo = resposta.tipo;
        }
      });
  
      showHideEdit();
      await lancamentoGet();
    } catch (error) {
      console.log(error);
    }
  }
 
  //Exclui lançamento
  const lancamentoDelete = async () => {
    try {
      const response = await Api.delete(`Transacoes/${lancamentoSelecionado.id}`);
      if (response.status === 200) {
        setData(data.filter(lancamento => lancamento.id !== lancamentoSelecionado.id));
        showHideDelete();
        console.log(lancamentoSelecionado.id);
      } else {
        console.log("Falha ao excluir transação");
      }
      await lancamentoGet();
    } catch (error) {
      console.log(error);
    }
  }
  
  const selecionarLancamento = (usuario: SetStateAction<ILancamento>, caso: string) => {
    setLancamentoSelecionado(usuario);
      (caso === "Editar") &&
        showHideEdit();
      (caso === "Excluir") &&
        showHideDelete();
  }

  const getTipoLabel = (tipo: number) => {
    return tipo === 1 ? 'Crédito' : tipo === 2 ? 'Débito' : '';
  };
  
  const handleChangeDate = (date: Date | Date[]) => {
    setLancamentoSelecionado({
      ...lancamentoSelecionado,
      data: Array.isArray(date) ? date[0] : date,
    });
  };
  
  useEffect(()=>{
    const fetchData = async () => {
      await lancamentoGet();
    }
    fetchData();
  }, [])

  return (
    <>
      <Navmenu />
      <div className="container">
        <h3>Lançamentos</h3>
        <Row className="justify-content-left">
          <Col xs="auto" className='d-flex align-items-end justify-content-end my-3'>
            <Button className="btn btn-success" onClick={showHideInsert}>
              {'Cadastrar'}
            </Button>
          </Col>
        </Row>
        <Table striped bordered hover>
          <thead>
            <tr>
              <th>Data</th>
              <th>Descricao</th>
              <th>Valor</th>
              <th>Tipo</th>
            </tr>
          </thead>
          <tbody>
              {Array.isArray(data) ? (
                data.map(lancamento => (
                  <tr key={lancamento.id}>
                    <td>{lancamento.data ? format(new Date(lancamento.data), 'dd/MM/yyyy') : ''}</td>
                    <td>{lancamento.descricao}</td>
                    <td>{lancamento.valor !== undefined ? lancamento.valor.toFixed(2) : ''}</td>
                    <td>{getTipoLabel(lancamento.tipo)}</td>
                    <td>
                      <button className="btn btn-primary" onClick={() => selecionarLancamento(lancamento, "Editar")}>Editar</button>{"  "}
                      <button className="btn btn-secondary" onClick={() => selecionarLancamento(lancamento, "Excluir")}>Excluir</button>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={2}>Nenhum resultado encontrado.</td>
                </tr>
              )}
          </tbody>
        </Table>     

        <Modal isOpen={showInsert}>
          <ModalHeader>Cadastrar Lançamento</ModalHeader>
          <ModalBody>
            <div className="form-group">
              <label>Data: </label>
              <br />
              <DateTimePicker selectedData={lancamentoSelecionado.data ? new Date(lancamentoSelecionado.data) : new Date()} handlerData={handleChangeDate} />
              <br />
              <label>Descrição: </label>
              <br />
              <input type="text" className="form-control" name="descricao"  onChange={handleChange}/>
              <br />
              <label>Valor: </label>
              <br />
              <input type="text" className="form-control" name="valor"  onChange={handleChange}/>
              <br />
              <label>Tipo: </label>
              <br />
              <SelectTipo status={lancamentoSelecionado.tipo} handlerStatus={(value) => handleChange({ target: { name: 'tipo', value } })} />
              <br />
            </div>
          </ModalBody>
          <ModalFooter>
            <button className="btn btn-primary" onClick={()=>canalPost()}>Salvar</button>{"   "}
            <button className="btn btn-danger" onClick={()=>showHideInsert()}>Cancelar</button>
          </ModalFooter>
        </Modal>

        <Modal isOpen={showEdit}>
          <ModalHeader>Editar Lançamento</ModalHeader>
          <ModalBody>
            <div className="form-group">
              <input type="text" className="form-control" readOnly hidden value={lancamentoSelecionado && lancamentoSelecionado.id}/>       
              <label>Data: </label>
              <br />
              <DateTimePicker selectedData={lancamentoSelecionado.data ? new Date(lancamentoSelecionado.data) : new Date()} handlerData={handleChangeDate} />
              <br />
              <label>Descrição: </label>
              <br />
              <input type="text" className="form-control" name="descricao" onChange={handleChange} value={lancamentoSelecionado && lancamentoSelecionado.descricao}/>
              <br />
              <label>Valor: </label>
              <br />
              <input type="text" className="form-control" name="valor" onChange={handleChange} value={lancamentoSelecionado && lancamentoSelecionado.valor !== undefined ? lancamentoSelecionado.valor.toFixed(2) : null}/>
              <br />
              <label>Tipo: </label>
              <br />
              <SelectTipo status={lancamentoSelecionado.tipo} handlerStatus={(value) => handleChange({ target: { name: 'tipo', value } })}/>
              <br />
            </div>
          </ModalBody>
          <ModalFooter>
            <button className="btn btn-primary" onClick={()=>canalPut()}>Salvar</button>{"   "}
            <button className="btn btn-danger" onClick={()=>showHideEdit()}>Cancelar</button>
          </ModalFooter>
        </Modal>

        <Modal isOpen={showDelete}>
          <ModalBody>
            Confirma a exclusão do lançamento: {lancamentoSelecionado && lancamentoSelecionado.descricao} ?
          </ModalBody>
          <ModalFooter>
            <button className="btn btn-danger" onClick={()=>lancamentoDelete()}>
              Sim
            </button>
            <button className="btn btn-secondary" onClick={()=>showHideDelete()}>
              Não
            </button>
          </ModalFooter>
        </Modal>

      </div>
    </>
  );
}
