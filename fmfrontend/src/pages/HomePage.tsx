import { Navmenu } from '../components/Nav/nav';
import background from '../assets/background.jpg';

export function HomePage() {
  const backgroundStyle = {
    backgroundImage: `url(${background})`,
    backgroundSize: 'cover',
    backgroundRepeat: 'no-repeat',
    backgroundPosition: 'center',
    height: '100vh',
  };

  return (
    <>
    <div style={backgroundStyle}>
      <Navmenu />
    </div>
    </>
  );
}
