import { useEffect, useState } from 'react';
import { Api } from '../providers';
import { DateTimePicker } from '../components/DateTimePicker/DateTimePicker';
import { Navmenu } from '../components/Nav/nav';
import { Col, Row, Table, Form, Button } from 'react-bootstrap';
import { format } from 'date-fns';

export function RelatorioPage(props: any) { 

  function getToday() {
    return new Date();
  }
  
  const [data, setData] = useState<any>();
  const [selectedData, setSelectedData] = useState(getToday());
  const [status, setStatus] = useState('');
  
  const handlerData = (data: Date) => {
    console.log("handlerData = " + data);
    setSelectedData(data);
  }

  const handlerStatus = (status: string) => {
    console.log("handlerStatus = " + status);
    setStatus(status);
  }

  const formattedDate = format(new Date(selectedData), 'yyyy-MM-dd');
  //console.log("data = " + formattedDate);

  const lancamentoGet = async () => {
    try {
      const response = await Api.get(`Transacoes/gerarRelatorioDiario?data=${formattedDate}`);
      
      // Acessando as propriedades do objeto 'result' na resposta
      const data = response.data.result.data;
      const saldoDoDia = response.data.result.saldoDoDia;
  
      // Usando os valores obtidos conforme necessário
      console.log("data =", data);
      console.log("saldo do dia =", saldoDoDia);
  
      // Atualizando o estado ou realizando outras ações com os dados
      setData(response.data);
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      await lancamentoGet();
    }
    fetchData();
  }, [])  

  return (
    <>
    <Navmenu />
    <div className="container">
      <h3>Relatório Diário</h3>
      <Form>
        <Row className="justify-content-left">
          <Col sm={2} xs="auto" className="my-3">
            <Form.Label htmlFor="dateTimePicker">Data</Form.Label>
            <DateTimePicker selectedData={selectedData} handlerData={handlerData}/>
          </Col>
          <Col xs="auto" className='d-flex align-items-end justify-content-end my-3'>
            <Button className="btn btn-success" onClick={lancamentoGet}>
              {'Visualizar'}
            </Button>
          </Col>
        </Row>
      </Form>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Data</th>
            <th>Saldo</th>
          </tr>
        </thead>
        <tbody>
          {data && data.result ? (
            <tr>
              <td>{format(new Date(data.result.data), 'dd/MM/yyyy')}</td>
              <td>{data.result.saldoDoDia.toFixed(2)}</td>
            </tr>
          ) : (
            <tr>
              <td colSpan={2}>Nenhum resultado encontrado.</td>
            </tr>
          )}
        </tbody>
      </Table>
    </div>
    </>
  );
}
