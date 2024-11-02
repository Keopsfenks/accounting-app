import React from 'react';
import Login from "./Login.jsx";
import "./authenticator.css"
import Register from "./Register.jsx";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faWhatsapp } from '@fortawesome/free-brands-svg-icons';

const Authenticator = () => {
	return (
		<section className="container">
			<div className="form-container">
				< Register/>
				<hr/>
				<p className={"help-parag"}>Yardıma ihtiyacınız mı var bize aşağıdaki bağlantılardan
					ulaşabilirsiniz.</p>
				<div className="whatsapp-btn">
					<a href={"#"} className={"icon"}>
						<FontAwesomeIcon icon={faWhatsapp}></FontAwesomeIcon> Whatsapp
					</a>
				</div>
			</div>
		</section>
	);
};

export default Authenticator;