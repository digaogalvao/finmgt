import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import ptBR from 'date-fns/locale/pt-BR';

type Props = {
  selectedData: Date;
  handlerData: (data: Date) => void
};

export function DateTimePicker({ selectedData,  handlerData}: Props) {
  const today = new Date();

  const selectDateHandler = (d: Date) => {
    console.log("selectDateHandler = " + new Date());
    handlerData(d);
  };
  
  return (
    <DatePicker
        dateFormat="dd/MM/yyyy"
        selected={selectedData}
        onChange={selectDateHandler}
        showFullMonthYearPicker
        locale={ptBR}
        className="form-control"
      />
  );
};