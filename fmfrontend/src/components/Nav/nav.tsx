import { Container, Nav, Navbar, NavDropdown } from 'react-bootstrap';

export function Navmenu() {
  return (
    <Navbar className="navbar navbar-dark bg-primary py-4"  expand="lg">
      <Container>
        <Navbar.Brand>
        <svg xmlns="http://www.w3.org/2000/svg" id="Layer_1" data-name="Layer 1" viewBox="0 0 24 24" width="40px" height="40px">
          <path d="M12,0C5.383,0,0,5.383,0,12s5.383,12,12,12,12-5.383,12-12S18.617,0,12,0Zm0,22c-5.514,0-10-4.486-10-10S6.486,2,12,2s10,4.486,10,10-4.486,10-10,10Zm4-8c0,1.654-1.346,3-3,3v1c0,.553-.447,1-1,1s-1-.447-1-1v-1h-.268c-1.067,0-2.063-.574-2.598-1.499-.277-.479-.113-1.09,.364-1.366,.479-.279,1.091-.113,1.366,.364,.179,.31,.511,.501,.867,.501h2.268c.552,0,1-.448,1-1,0-.378-.271-.698-.644-.76l-3.041-.507c-1.342-.223-2.315-1.373-2.315-2.733,0-1.654,1.346-3,3-3v-1c0-.552,.447-1,1-1s1,.448,1,1v1h.268c1.067,0,2.063,.575,2.598,1.5,.277,.478,.113,1.089-.364,1.366-.48,.277-1.091,.113-1.366-.365-.179-.309-.511-.5-.867-.5h-2.268c-.552,0-1,.449-1,1,0,.378,.271,.698,.644,.76l3.041,.507c1.342,.223,2.315,1.373,2.315,2.733Z"/>
        </svg>
        </Navbar.Brand>
        <Navbar.Brand href="/" title='Financial Management'>FIN MGT</Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto">
            <Nav.Link href="/lancamento">Lançamento</Nav.Link>
            <Nav.Link href="/relatorio" >Relatório</Nav.Link>
          </Nav>
          <Nav>
            <NavDropdown title="Admin" id="basic-nav-dropdown">
              <NavDropdown.Item href="/">Administração</NavDropdown.Item>
            </NavDropdown>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>    
  );
}
